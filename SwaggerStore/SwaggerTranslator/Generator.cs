using SwaggerTranslator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerTranslator
{
    public class AndroidGenerator
    {

        public SwaggerModel Model { get; set; }

        public static AndroidGenerator Create(string json) {

            AndroidGenerator g = new AndroidGenerator();
            g.Model = SwaggerModel.From(json);

            return g;

        }


    }
}
