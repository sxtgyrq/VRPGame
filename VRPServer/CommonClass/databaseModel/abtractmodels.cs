using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass.databaseModel
{
    public class abtractmodels
    {
        public string modelName { get; set; }
        public string modelType { get; set; }
        //public string imageBase64 { get; set; }
        //public string objText { get; set; }
        //public string mtlText { get; set; }
        //public string animation { get; set; }
        public string amID { get; set; }
        public string author { get; set; }
        public int amState { get; set; }
    }
    public class abtractmodelsPassData
    {
        public string mtlText 
        {
            get 
            {
                var result = "";
                for (int i = 0; i < this.mtlTexts.Length; i++)
                {
                    result += mtlTexts[i];
                    if (i != this.mtlTexts.Length - 1)
                    {
                        result += Environment.NewLine;
                    }
                }
                return result;
            }
        }

        public string objText
        {
            get
            {
                var result = "";
                for (int i = 0; i < this.objTexts.Length; i++)
                {
                    result += objTexts[i];
                    if (i != this.objTexts.Length - 1) 
                    {
                        result+= Environment.NewLine;
                    }
                }
                return result;
            }
        }

        public string imageBase64
        {
            get
            {
                var result = "";
                for (int i = 0; i < imgBase64.Length; i++)
                {
                    result += imgBase64[i];
                }
                return result;
            }
        }

        public string modelName { get; set; }
        public string modelType { get; set; }
        //public string imageBase64 { get; set; }
        public string[] objTexts { get; set; }
        public string[] mtlTexts { get; set; }
        public string[] imgBase64 { get; set; }
        // public string animation { get; set; }
    }
}
