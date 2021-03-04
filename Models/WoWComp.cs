using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PassionProjectTwo.Models
{
    public class WoWComp
    {
        [Key]
        public int CompID { get; set; }

        public string CompName { get; set; }

        public string CompClass1 { get; set; }

        public string CompClass2 { get; set; }

        public string CompClass3 { get; set; }

        //A team can have up to three classes
        public ICollection<WoWClass> Classes { get; set; }


    }

}