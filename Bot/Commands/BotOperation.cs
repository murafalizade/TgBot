﻿using Bot.DTOs;
using Bot.ScheduleServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Commands
{
    class BotOperation
    {
        private readonly Schedule _services;
        BotOperation(Schedule schedule)
        {
            _services = schedule;
        }
        public static ReplyKeyboardMarkup OrderCrypto()
        {
            CryptoData crypto = BotCommand.AllCrypto();
            var rkm = new ReplyKeyboardMarkup();
            var rows = new List<KeyboardButton[]>();
            var cols = new List<KeyboardButton>();
            foreach (Crypto p in crypto.Data)
            {
                string name = p.slug;
                cols.Add(new KeyboardButton("" + name));
                rows.Add(cols.ToArray());
                cols = new List<KeyboardButton>();
            }
            rkm.Keyboard = rows.ToArray();
            return rkm;
        }
        public static string SendPrice(string slug)
        {
            string price = BotCommand.GetPrice(slug);
            if (price == null || price == "USD")
            {
                return "CryptoValue doesn't found.Please try again!";
            }
            else
            {
                return price;
            }
        }
        public static async Task<string> ReminderPrice(string text)
        {
            string[] words = text.Split(' ');
            string slug = words[1];
            decimal price = Convert.ToDecimal(words[2]);
            if (slug == null || slug == "" || price == 0)
            {
                return "Please try again";
            }
            await _services.SchedulJob(slug,price);
            return "Congrulatulation!!! Your cryptovalue achive your goal.Come back.";
        }
    }
}