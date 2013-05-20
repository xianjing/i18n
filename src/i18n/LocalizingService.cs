﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using i18n.Core;
using i18n.Core.Models;

namespace i18n
{
    public class LocalizingService : ILocalizingService
    {
        private readonly I18NMessagesRepository i18NMessagesRepository;

        public LocalizingService(I18NMessagesRepository i18NMessagesRepository)
        {
            this.i18NMessagesRepository = i18NMessagesRepository;
        }

        public virtual string GetBestAvailableLanguageFrom(string[] languages)
        {
            var cultureInfos = ToCultureInfos(languages);
            return i18NMessagesRepository.GetBest(cultureInfos, DefaultSettings.DefaultTwoLetterISOLanguageName);
        }

        public string GetText(string key, string[] languages)
        {
            var cultureInfos = ToCultureInfos(languages);
            return i18NMessagesRepository.FindByCultures(key, cultureInfos);
        }

        public IList<I18NMessage> GetAll(string[] languages)
        {
            var culture = GetBestAvailableLanguageFrom(languages);
            return i18NMessagesRepository.Get(culture).Values.ToList();
        }

        private static IEnumerable<CultureInfo> ToCultureInfos(string[] languages)
        {
            return languages.Where(language => !string.IsNullOrWhiteSpace(language)).Select(language1 =>
                {
                    var semiColonIndex = language1.IndexOf(';');
                    var l = semiColonIndex < 0 ? language1: language1.Substring(0, semiColonIndex);
                    return new CultureInfo(CultureInfo.CreateSpecificCulture(l).Name, true);
                });
        }
    }
}
