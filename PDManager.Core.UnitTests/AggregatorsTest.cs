using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDManager.Core.Aggregators;
using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.UnitTests
{
    [TestClass]
    public class AggregatorsTest
    {

        private AggrConfigDefinition CreateDummyAggregation()
        {


            AggrConfigDefinition def = new AggrConfigDefinition()
            {

                AggregationType = "time",
                Beta = 0.0,
                Name="UPDRS",
                Code="UPDRS",
                Description="UPDRS PDManager Score",
                Version="1.0",
                Variables = new List<AggrConfigVarDefinition>()
                {

                    new AggrConfigVarDefinition()
                    {
                        Weight=1,
                        Code="STBRADS30",
                        Uri="api/observations"

                    },

                      new AggrConfigVarDefinition()
                    {
                        Weight=-1,
                        Code="STDYS30",
                        Uri="api/observations"

                    },


                }


            };

            return def;

        }

        
        [TestMethod]
        public void SaveAggregation()
        {

            var def = CreateDummyAggregation();

            AggrConfigDefinition.SaveToFile(def, "updrs.json");

            var defTarget=AggrConfigDefinition.LoadFromFile( "updrs.json");

            Assert.AreEqual(def.AggregationType, defTarget.AggregationType);


        }





    }
}
