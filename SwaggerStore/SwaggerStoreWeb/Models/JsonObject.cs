using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace SwaggerStoreWeb.Models
{
    public class JsonObject : IEnumerable<KeyValuePair<String,object>>
    {
        Dictionary<String, object> values;

        public JsonObject(Dictionary<String,object> values)
        {
            this.values = values;

            foreach (var p in values.ToList()) {
                var d = p.Value as Dictionary<string, object>;
                if (d != null) {
                    this.values[p.Key] = new JsonObject(d);
                }


                var a = p.Value as IList;
                if (a != null) {
                    this.values[p.Key] = new List<JsonObject>(a.Cast<Dictionary<string,object>>().Select(n=> new JsonObject(n)));
                }
            }
        }


        //public object this[string key] {
        //    get {
        //        object v = null;

        //        values.TryGetValue(key, out v);
        //        return v;
        //    }
        //}


        public string StringValue(String key) {
            object v = null;
            values.TryGetValue(key, out v);
            return v as string;
        }

        public JsonObject ObjectValue(string key) {
            object v = null;
            values.TryGetValue(key, out v);
            return v as JsonObject;
        }

        public bool BoolValue(string key) {
            object v = null;
            if (values.TryGetValue(key, out v))
                return (bool)v;
            return false;
        }

        public IEnumerable<JsonObject> ArrayValue(string key)
        {
            object v = null;
            values.TryGetValue(key, out v);
            return ((System.Collections.IEnumerable)v).Cast<JsonObject>();
        }

        public static JsonObject Parse(string text) {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return new JsonObject((Dictionary<String,object>)js.DeserializeObject(text));
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.values.GetEnumerator();
        }
    }
}