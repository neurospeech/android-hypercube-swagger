using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace SwaggerStoreWeb.Models
{
    public class SwaggerModel
    {

        private JsonObject swagger = null;

        private SwaggerModel(String text)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            swagger = JsonObject.Parse(text);

            var info = swagger.ObjectValue("info");
            Version = info.StringValue("version");
            Title = info.StringValue("title");

            var paths = swagger.ObjectValue("paths");

            Methods = paths.Select(x => new SwaggerMethod(x.Key,x.Value)).ToList();

        }


        public static SwaggerModel From(string text) {

            return new SwaggerModel(text);

        }


        public string Version { get; set; }

        public string Title { get; set; }

        public List<SwaggerMethod> Methods { get; set; }

    }

    public class SwaggerMethod  {
        

        public SwaggerMethod(string name, object value)
        {
            Path = name;

            var x = (JsonObject)value;

            var httpMethod = x.FirstOrDefault();
            HttpMethod = httpMethod.Key;

            var method = (JsonObject)httpMethod.Value;

            Description = method.StringValue("description");

            var parameters = method.ArrayValue("parameters");
            var responses = method.ObjectValue("responses");

            Parameters = parameters.Select(p => new SwaggerParameter(p)).ToList();

            Responses = responses.Select(r => new SwaggerResponse(r.Key,r.Value)).ToList();
        }

        public string Description { get; set; }

        public string Path { get; set; }

        public string HttpMethod { get; set; }

        public List<SwaggerParameter> Parameters { get; set; }

        public List<SwaggerResponse> Responses { get; set; }


    }


    public class SwaggerResponse {

        public SwaggerResponse(string name, object value)
        {
            Response = name;
            var d = (JsonObject)value;
            Description = d.StringValue("description");
            var schema = d.ObjectValue("schema");
            if (schema != null) {
                if ((schema.StringValue("type")) == "array")
                {
                    Schema = new SwaggerSchemaArray(schema);
                }
                else {
                    Schema = new SwaggerSchema(schema);
                }
            }
        }


        public string Response { get; set; }

        public string Description { get; set; }

        public SwaggerSchema Schema { get; set; }

    }


    public class SwaggerSchema {

        public SwaggerSchema(JsonObject schema)
        {
            Name = schema.StringValue("title");

            var props = schema.ObjectValue("properties");

            Properties = props.Select( x=> {
                var m = new SwaggerMember((JsonObject)x.Value);
                m.Name = (string)x.Key;
                return m;
            } ).ToList();
        }

        public string Name { get; set; }

        public List<SwaggerMember> Properties { get; set; }

    }

    public class SwaggerSchemaArray : SwaggerSchema {

        public SwaggerSchemaArray(JsonObject schema):base(schema.ObjectValue("items"))
        {
            ArrayName = schema.StringValue("title");

        }

        public string ArrayName { get; set; }

    }

    public class SwaggerMember
    {

        public SwaggerMember(JsonObject token)
        {
            Name = token.StringValue("name");
            Type = token.StringValue("type");
            Format = token.StringValue("format");
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Format { get; set; }
    }

    public class SwaggerParameter : SwaggerMember  {

        public SwaggerParameter(JsonObject token):base(token)
        {

            Required = token.BoolValue("required");

            string p = token.StringValue("in");
            if (p != null) {
                p = p.ToLower();
                switch (p)
                {
                    case "query":
                        Position = Position.Query;
                        break;
                    case "formdata":
                        Position = Position.FormData;
                        break;
                    case "header":
                        Position = Position.Header;
                        break;
                    default:
                        break;
                }
            }
        }

        public bool Required { get; set; }

        public Position Position { get; set; }

    }

    public enum Position {
        None,
        Query,
        FormData,
        Header
    }


}