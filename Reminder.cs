﻿using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ApptReminderWindowsClient
{
    public class Reminder
    {
        private readonly string DefaultTemplateName = "|";
        public Reminder(List<KeyValuePair<string, string>> templateNames)
        {
            Days = 0;
            Hours = -1;
            Template = DefaultTemplateName;
            TemplateNames = templateNames;
            MainContact = null;
            SecondaryContact = null;
        }

        public Reminder(dynamic data, List<KeyValuePair<string, string>> templateNames)
        {
            Days = (int)data.daysBeforeApt;
            Hours = ((IDictionary<string, object>)data).ContainsKey("hoursBeforeApt") ? (int)data.hoursBeforeApt : -1;
            Template = ((IDictionary<string, object>)data).ContainsKey("template") ? data.template : DefaultTemplateName;
            TemplateNames = templateNames;
            if (data.contactTypes.Count == 0)
            {
                MainContact = null;
                SecondaryContact = null;
            }
            else
            {
                MainContact = data.contactTypes[0][0];
                SecondaryContact = data.contactTypes[0].Count > 1 ? data.contactTypes[0][1] : null;
            }
        }

        public string Id { get; } = Guid.NewGuid().ToString();
        public int Days { get; set; }
        public int Hours { get; set; }
        public string Template { get; set; }
        public List<KeyValuePair<string, string>> TemplateNames { get; set; }
        public string MainContact { get; set; }
        public string SecondaryContact { get; set; }

        public dynamic Convert()
        {
            dynamic data = new ExpandoObject();
            data.daysBeforeApt = Days;
            if (Hours != -1) data.hoursBeforeApt = Hours;
            if (Template != DefaultTemplateName) data.template = Template;
            List<string> contactTypes = new List<string>();
            if (MainContact != null) contactTypes.Add(MainContact);
            if (SecondaryContact != null) contactTypes.Add(SecondaryContact);
            if (contactTypes.Count > 0) data.contactTypes = new List<List<string>> { contactTypes };
            else data.contactTypes = new List<List<string>>();
            return data;
        }
    }
}
