using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using PDManager.Core.Common.Models;
using PDManager.Core.DSS.Helpers;
using System.IO;

namespace PDManager.Core.UnitTests
{
    [TestClass]
    public class DSSTest
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void DSSValueCollection_SchemaSave_Test()
        {



            var jsonSchemaGenerator = new JsonSchemaGenerator();
            var myType = typeof(DSSValueCollection);
            var schema = jsonSchemaGenerator.Generate(myType);
            schema.Title = myType.Name;
            var writer = new StringWriter();
            var jsonTextWriter = new JsonTextWriter(writer);
            schema.WriteTo(jsonTextWriter);
            dynamic parsedJson = JsonConvert.DeserializeObject(writer.ToString());
            var prettyString = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            var fileWriter = new StreamWriter("dssValueSchema.txt");
            fileWriter.WriteLine(schema.Title);
            fileWriter.WriteLine(new string('-', schema.Title.Length));
            fileWriter.WriteLine(prettyString);
            fileWriter.Close();


            Assert.IsTrue(true);




        }
    }
}
