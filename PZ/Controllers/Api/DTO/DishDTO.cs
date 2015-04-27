using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PZ.Controllers.Api.DTO
{
    [Serializable]
    [KnownType(typeof(DishBundleDTO))]
    [DataContract(Namespace = "", Name = "items")]
    public class DishBundleDTO
    {
        [DataMember]
        public DishDTO[] dishes;
    }

    [Serializable]
    [KnownType(typeof(DishDTO))]
    [DataContract(Namespace = "", Name="item")]
    public class DishDTO
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public decimal price { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public int? menuid { get; set; }
    }
}