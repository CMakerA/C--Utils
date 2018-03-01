using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator : MonoBehaviour {
    #region Translator Classes
    public class Item
    {
        public readonly string tag, value;

        public Item(string tag, string value)
        {
            this.tag = tag;
            this.value = value;
        }
    }

    public class LangCode
    {
        string languageName, languageCountryName;

        public readonly string languageCode;

        public LangCode(string languageName, string languageCountryName)
        {
            this.languageName = languageName;
            this.languageCountryName = languageCountryName;

            languageCode = languageName.Substring(0, 2) + "-" + languageCountryName.Substring(0, 2);
        }
    }

    public class Language
    {
        public readonly LangCode langCode;
        public readonly List<Item> items;

        public Language(LangCode langCode, List<Item> items)
        {
            this.langCode = langCode;
            this.items = items;
        }
    }
    #endregion

    #region Exceptions
    public class LanguageAlreadyExistsException : Exception { }
    public class TagNotFoundException : Exception { }
    public class CurrentLanguageNotSetException : Exception { }
    #endregion

    private static List<Language> languages;

    #region GetLanguage
    /// <summary>
    /// Search a language by its langCode::LangCode
    /// </summary>
    /// <param name="langCode">The LangCode to search</param>
    /// <returns>Can return null if langCode not found</returns>
    public static Language GetLanguage(LangCode langCode)
    {
        foreach(Language language in languages) if (language.langCode == langCode) return language;

        return null;
    }

    /// <summary>
    /// Search a language by its langCode::string
    /// </summary>
    /// <param name="langCode">The LangCode to search</param>
    /// <returns>Can return null if langCode not found</returns>
    public static Language GetLanguage(string langCode)
    {
        foreach (Language language in languages) if (language.langCode.languageCode.Equals(langCode, System.StringComparison.CurrentCultureIgnoreCase)) return language;

        return null;
    }

    /// <summary>
    /// Get all the declared languages.
    /// </summary>
    /// <returns>List<Language> of languages.</returns>
    public static List<Language> GetLanguages()
    {
        return languages;
    }
    #endregion

    /// <summary>
    /// Adds a new language
    /// </summary>
    /// <exception cref="LanguageAlreadyExistsException">If language already declared</exception>
    /// <param name="language"></param>
    public static void DeclareLanguage(Language language)
    {
        foreach (Language lang in languages) if (language.langCode.languageCode.Equals(language.langCode.languageCode)) throw new LanguageAlreadyExistsException();

        languages.Add(language);
    }


    public static Language currentLanguage = null;

    /// <summary>
    /// Gets the content from a language of a tag
    /// </summary>
    /// <param name="tag">The tag to search</param>
    /// <param name="language">The language</param>
    /// <exception cref="TagNotFoundException">If tag doesn't exist in the current language</exception>
    /// <returns></returns>
    public static String Get(string tag, Language language)
    {
        foreach (Item item in language.items) if (item.tag.Equals(tag, StringComparison.CurrentCultureIgnoreCase)) return item.value;

        throw new TagNotFoundException();
    }

    /// <summary>
    /// Gets the content from a language of a tag
    /// </summary>
    /// <param name="tag">The tag to search</param>
    /// <exception cref="TagNotFoundException">If tag doesn't exist in the current language</exception>
    /// <exception cref="CurrentLanguageNotSetException">If currentLanguage not set</exception>
    /// <returns></returns>
    public static String Get(string tag)
    {
        if (currentLanguage == null) throw new CurrentLanguageNotSetException();

        foreach (Item item in currentLanguage.items) if (item.tag.Equals(tag, StringComparison.CurrentCultureIgnoreCase)) return item.value;

        throw new TagNotFoundException();
    }
}
