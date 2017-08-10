using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Builder.Luis;

namespace Lyuba02_with_luis_ai.Dialogs
{
    [LuisModel("YourLuisAiId", "YourLuisAiPassword")]
    [Serializable]
    public class RootDialog : LuisDialog<Object>
    {

        [LuisIntent("BlaBla")]
        public async Task BlaBla(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Хорошо. А почему вы спрашиваете?");
            context.Wait(MessageReceived);
        }


        [LuisIntent("HELLOW")]
        public async Task Hello(IDialogContext context, LuisResult result)
        {

            var greeting = new string[]
            {
                "Привет!",
                "Добрый день.",
                "Здравствуйте!",
                "Чем могу помочь?"
            };

            var mes = greeting[(new Random()).Next(greeting.Length - 1)];
            await context.PostAsync(mes);
            context.Wait(MessageReceived);
        }


        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Не понимаю :(");
            context.Wait(MessageReceived);
        }


        [LuisIntent("numbers")]
        public async Task Numbers(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(db.AllNumbers());
            context.Wait(MessageReceived);
        }


        [LuisIntent("EditNumber")]
        public async Task EditNumber(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Введите номер телефона\nбез 8: 9012345678");
            context.Wait(EnterNumber);
        }


        [LuisIntent("mails")]
        public async Task Mails(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(db.AllMails());
            context.Wait(MessageReceived);
        }


        [LuisIntent("EditMail")]
        public async Task EditMail(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Введите адрес электронной почты:");
            context.Wait(EnterMail);
        }


        private bool numberOn = false;
        private bool mailOn = false;
        private string code;
        private string currentNumberOrMail;
        private int countForNumberAndMail = 0;
        private int countForCode = 0;
        private Random r = new Random();
        private DataBase db = new DataBase();
        private Helper helper = new Helper();
        private MailSender sender = new MailSender();


        private async Task EnterNumber(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

            var msg = await argument;
            string text = helper.DeleteAll(msg.Text);

            if (helper.DeleteAll(text).Equals("отмена"))
            {
                await context.PostAsync("Редактирование отменено.");
                countForCode = 0;
                countForNumberAndMail = 0;
                context.Wait(MessageReceived);
                return;
            }

            if (helper.IsPhoneNumber(text))
            {
                bool unq = db.IsExistNumber(text);

                if (unq)
                {
                    if (countForNumberAndMail < 4)
                    {
                        countForNumberAndMail++;
                        await context.PostAsync("Номер уже существует.\n" +
                            "Осталось попыток: " + (5 - countForNumberAndMail) + ".\nВведите номер заново.");
                        context.Wait(EnterNumber);
                    }
                    else
                    {
                        await context.PostAsync("Номер уже существует.\nОтмена.");
                        countForNumberAndMail = 0;
                        context.Wait(MessageReceived);
                    }
                }
                else
                {
                    code = r.Next(10_000, 99_999).ToString();
                    currentNumberOrMail = text;
                    numberOn = true;

                    //string myApiKey = "YourAPIKey"; //API ключ от сайта http://sms.ru/
                    //SmsRu sms = new SmsRu(myApiKey);
                    //var response = sms.Send("7" + text, code);

                    await context.PostAsync("На номер " + text + " отправлен код подтверждения.\n Введите код: " + code);
                    countForNumberAndMail = 0;
                    context.Wait(EnterCode);
                }

            }
            else
            {
                if (countForNumberAndMail < 4)
                {
                    countForNumberAndMail++;
                    await context.PostAsync("Введен некорректный номер мобильного телефона.\n" +
                        "Осталось попыток: " + (5 - countForNumberAndMail) + ".\nВведите номер заново.");
                    context.Wait(EnterNumber);
                }
                else
                {
                    await context.PostAsync("Введен некорректный номер мобильного телефона.\nОтмена.");
                    countForNumberAndMail = 0;
                    context.Wait(MessageReceived);
                }
            }
        }


        private async Task EnterCode(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

            var msg = await argument;
            string text = msg.Text;

            if (helper.DeleteAll(text).Equals("отмена"))
            {
                await context.PostAsync("Редактирование отменено.");
                numberOn = false;
                mailOn = false;
                countForCode = 0;
                countForNumberAndMail = 0;
                context.Wait(MessageReceived);
                return;
            }

            if (!code.Equals(text))
            {
                if (countForCode < 4)
                {
                    countForCode++;
                    await context.PostAsync("Введен Неправильный код.\n" +
                        "Осталось попыток: " + (5 - countForCode) + ".\nВведите код заново.");
                    context.Wait(EnterCode);
                }
                else
                {
                    await context.PostAsync("Неправильный код.\nОтмена.");
                    numberOn = false;
                    mailOn = false;
                    countForCode = 0;
                    context.Wait(MessageReceived);
                }
            }
            else
            {
                countForCode = 0;
                countForNumberAndMail = 0;
                // привязка к учетной записи

                if (numberOn)
                {
                    db.InsertInDbNumber(currentNumberOrMail);
                    await context.PostAsync("Номер мобильного телефона успешно сохранен.");
                }

                if (mailOn)
                {
                    db.InsertInDbMail(currentNumberOrMail);
                    await context.PostAsync("Адрес электронной почты успешно сохранен.");
                }

                numberOn = false;
                mailOn = false;
                
                context.Wait(MessageReceived);
            }
        }


        private async Task EnterMail(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

            var msg = await argument;
            string text = msg.Text;

            if (helper.DeleteAll(text).Equals("отмена"))
            {
                await context.PostAsync("Редактирование отменено.");
                countForCode = 0;
                countForNumberAndMail = 0;
                context.Wait(MessageReceived);
                return;
            }

            if (helper.IsMail(text))
            {

                bool unq = db.IsExistMail(text);

                if (unq)
                {
                    if (countForNumberAndMail < 4)
                    {
                        countForNumberAndMail++;
                        await context.PostAsync("Почта уже существует.\n" +
                            "Осталось попыток: " + (5 - countForNumberAndMail) + ".\nВведите почту заново.");
                        context.Wait(EnterMail);
                    }
                    else
                    {
                        await context.PostAsync("Почта уже существует.\nОтмена.");
                        countForNumberAndMail = 0;
                        context.Wait(MessageReceived);
                    }
                }
                else
                {
                    code = r.Next(10_000, 99_999).ToString();
                    currentNumberOrMail = text;
                    mailOn = true;

                    //await sender.SendCode(text, code);

                    await context.PostAsync("На почту " + text + " отправлен код подтверждения.\n Введите код: " + code);
                    countForNumberAndMail = 0;
                    context.Wait(EnterCode);
                }

            }
            else
            {
                if (countForNumberAndMail < 4)
                {
                    countForNumberAndMail++;
                    await context.PostAsync("Введена некорректная почта.\n" +
                        "Осталось попыток: " + (5 - countForNumberAndMail) + ".\nВведите почту заново.");
                    context.Wait(EnterMail);
                }
                else
                {
                    await context.PostAsync("Введена некорректная почта.\nОтмена.");
                    countForNumberAndMail = 0;
                    context.Wait(MessageReceived);
                }
            }
        }


    }



}