using Newtonsoft.Json;
using PDManager.Core.Aggregators;
using PDManager.Core.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Aggregators.Testing
{
    /// <summary>
    /// Dummy Aggr Definition Provider
    /// </summary>
    public class DummyAggrDefinitionProvider : IAggrDefinitionProvider
    {

        /// <summary>
        /// ONOFF Aggregation as a function of time
        /// </summary>
        /// <returns></returns>
        public static AggrConfig CreateOnOffAggregation()
        {

            AggrConfig def = new AggrConfig()
            {

                AggregationType = "time",
                Name = "ONOFF",
                Code = "ONOFF",
                Description = "UPDRS PDManager Score",
                Version = "1.0",             
                ThresholdType = "fixed",
                Threshold = true,
                ThresholdValue = 15,
                Beta = 3.5621,
                MetaScale = 1.0,
                Variables = new List<AggrVariable>()
                {

                    new AggrVariable()
                    {
                        Weight=8.2422,
                        Code="STBRAD30",
                        Source="observation"

                    },
                    new AggrVariable()
                    {
                        Weight=4.763,
                        Code="STUPDRSG",
                        Source="observation"

                    },

                      new AggrVariable()
                    {
                        Weight=4.0556,
                        Code="STFOG",
                        Source="observation"

                    },
                       new AggrVariable()
                    {
                        Weight=1.4900,
                        Code="STTRMA30",
                        Source="observation"

                    },

                        new AggrVariable()
                    {
                        Weight=2.5942,
                        Code="STTRMC30",
                        Source="observation"

                    },


                      new AggrVariable()
                    {
                        Weight=2.7079,
                        Code="STDYS30",
                        Source="observation"

                    },


                }


            };

            return def;

        }


        /// <summary>
        /// Off Time Aggregation
        /// </summary>
        /// <returns></returns>
        public static AggrConfig CreateOffTimeAggregation()
        {

            AggrConfig def = new AggrConfig()
            {

                AggregationType = "time",
                Name = "OFFTIME",
                Code = "STOFFDUR",
                Description = "UPDRS PDManager Score",
                Version = "1.0",
                MetaAggregationType="average",
                ThresholdType = "fixed",
                Threshold = true,
                ThresholdValue = 15,
                Beta = 3.5621,
                MetaScale=4.0,
                Variables = new List<AggrVariable>()
                {

                    new AggrVariable()
                    {
                        Weight=8.2422,
                        Code="STBRAD30",
                        Source="observation"

                    },
                    new AggrVariable()
                    {
                        Weight=4.763,
                        Code="STUPDRSG",
                        Source="observation"

                    },

                      new AggrVariable()
                    {
                        Weight=4.0556,
                        Code="STFOG",
                        Source="observation"

                    },
                       new AggrVariable()
                    {
                        Weight=1.4900,
                        Code="STTRMA30",
                        Source="observation"

                    },

                        new AggrVariable()
                    {
                        Weight=2.5942,
                        Code="STTRMC30",
                        Source="observation"

                    },


                      new AggrVariable()
                    {
                        Weight=2.7079,
                        Code="STDYS30",
                        Source="observation"

                    },


                }


            };

            return def;

        }


        /// <summary>
        /// Total UPDRS score
        /// </summary>
        /// <returns></returns>
        public static AggrConfig CreateUPDRSTOTALAggregation()
        {


            AggrConfig def = new AggrConfig()
            {

                AggregationType = "total",              
                Name = "UPDRS Total",
                Code = "UPDRSTOTAL",
                Description = "UPDRS PDManager Score",
                Version = "1.0",
                Beta = 3.5621,
                MetaScale = 1.0,
                Variables = new List<AggrVariable>()
                {

                    new AggrVariable()
                    {
                        Weight=8.2422,
                        Code="STBRAD30",
                        Source="observation"

                    },
                    new AggrVariable()
                    {
                        Weight=4.763,
                        Code="STUPDRSG",
                        Source="observation"

                    },

                      new AggrVariable()
                    {
                        Weight=4.0556,
                        Code="STFOG",
                        Source="observation"

                    },
                       new AggrVariable()
                    {
                        Weight=1.4900,
                        Code="STTRMA30",
                        Source="observation"

                    },

                        new AggrVariable()
                    {
                        Weight=2.5942,
                        Code="STTRMC30",
                        Source="observation"

                    },


                      new AggrVariable()
                    {
                        Weight=2.7079,
                        Code="STDYS30",
                        Source="observation"

                    },


                }


            };

            return def;

        }
        /// <summary>
        /// Day UPDRS
        /// </summary>
        /// <returns></returns>

        public static AggrConfig CreateUPDRSDAYAggregation()
        {


            AggrConfig def = new AggrConfig()
            {

                AggregationType = "day",
                Name = "UPDRS Total",
                Code = "UPDRSDAY",
                Description = "UPDRS PDManager Score",
                Version = "1.0",
                Beta = 3.5621,
                MetaScale = 1.0,
                Variables = new List<AggrVariable>()
                {

                    new AggrVariable()
                    {
                        Weight=8.2422,
                        Code="STBRAD30",
                        Source="observation"

                    },
                    new AggrVariable()
                    {
                        Weight=4.763,
                        Code="STUPDRSG",
                        Source="observation"

                    },

                      new AggrVariable()
                    {
                        Weight=4.0556,
                        Code="STFOG",
                        Source="observation"

                    },
                       new AggrVariable()
                    {
                        Weight=1.4900,
                        Code="STTRMA30",
                        Source="observation"

                    },

                        new AggrVariable()
                    {
                        Weight=2.5942,
                        Code="STTRMC30",
                        Source="observation"

                    },


                      new AggrVariable()
                    {
                        Weight=2.7079,
                        Code="STDYS30",
                        Source="observation"

                    },


                }


            };

            return def;

        }
     
        /// <summary>
        /// Time UPDRS
        /// </summary>
        /// <returns></returns>
        public static AggrConfig CreateUPDRSAggregation()
        {


            AggrConfig def = new AggrConfig()
            {

                AggregationType = "time",
                Name = "UPDRS",
                Code = "UPDRS",
                Description = "UPDRS PDManager Score",
                Version = "1.0",
                MetaScale = 1.0,
                Beta = 3.5621,
                Variables = new List<AggrVariable>()
                {

                    new AggrVariable()
                    {
                        Weight=8.2422,
                        Code="STBRAD30",
                        Source="observation"

                    },
                    new AggrVariable()
                    {
                        Weight=4.763,
                        Code="STUPDRSG",
                        Source="observation"

                    },

                      new AggrVariable()
                    {
                        Weight=4.0556,
                        Code="STFOG",
                        Source="observation"

                    },
                       new AggrVariable()
                    {
                        Weight=1.4900,
                        Code="STTRMA30",
                        Source="observation"

                    },

                        new AggrVariable()
                    {
                        Weight=2.5942,
                        Code="STTRMC30",
                        Source="observation"

                    },


                      new AggrVariable()
                    {
                        Weight=2.7079,
                        Code="STDYS30",
                        Source="observation"

                    },


                }


            };

            return def;

        }


        /// <summary>
        /// Time UPDRS
        /// </summary>
        /// <returns></returns>
        public static AggrConfig CreateFLUCTAggregation()
        {


            AggrConfig def = new AggrConfig()
            {

                AggregationType = "time",
                Name = "Fluctuations",
                Code = "STFLUCT",
                Description = "UPDRS Fluctuation Score based on MFI",
                Version = "1.0",
                MetaScale = 1.0,
                MetaAggregationType="mfi",
                Beta = 3.5621,
                Variables = new List<AggrVariable>()
                {

                    new AggrVariable()
                    {
                        Weight=8.2422,
                        Code="STBRAD30",
                        Source="observation"

                    },
                    new AggrVariable()
                    {
                        Weight=4.763,
                        Code="STUPDRSG",
                        Source="observation"

                    },

                      new AggrVariable()
                    {
                        Weight=4.0556,
                        Code="STFOG",
                        Source="observation"

                    },
                       new AggrVariable()
                    {
                        Weight=1.4900,
                        Code="STTRMA30",
                        Source="observation"

                    },

                        new AggrVariable()
                    {
                        Weight=2.5942,
                        Code="STTRMC30",
                        Source="observation"

                    },


                      new AggrVariable()
                    {
                        Weight=2.7079,
                        Code="STDYS30",
                        Source="observation"

                    },


                }


            };

            return def;

        }
        /// <summary>
        /// Get Json Config From Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetJsonConfigFromCode(string code)
        {
          

            if(code=="UPDRS")
            {

                return JsonConvert.SerializeObject(CreateUPDRSAggregation());

            }
            else if (code == "UPDRSTOTAL")
            {

                return JsonConvert.SerializeObject(CreateUPDRSTOTALAggregation());

            }
            else if (code == "UPDRSDAY")
            {

                return JsonConvert.SerializeObject(CreateUPDRSDAYAggregation());

            }
            else if (code == "STOFFDUR")
            {

                return JsonConvert.SerializeObject(CreateOffTimeAggregation());

            }
            else if (code == "STFLUCT")
            {

                return JsonConvert.SerializeObject(CreateFLUCTAggregation());

            }
            else if(code=="ONOFF")
            {
                return JsonConvert.SerializeObject(CreateOnOffAggregation());
            }

            throw new KeyNotFoundException();

        }



        /// <summary>
        /// Get Aggregation Config From Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public AggrConfig GetConfigFromCode(string code)
        {


            if (code == "UPDRS")
            {

                return CreateUPDRSAggregation();

            }
            else if (code == "UPDRSTOTAL")
            {

                return CreateUPDRSTOTALAggregation();

            }
            else if (code == "UPDRSDAY")
            {

                return CreateUPDRSDAYAggregation();

            }
            else if (code == "STOFFDUR")
            {

                return (CreateOffTimeAggregation());

            }
            else if (code == "STFLUCT")
            {

                return (CreateFLUCTAggregation());

            }
            else if (code == "ONOFF")
            {
                return CreateOnOffAggregation();
            }

            throw new KeyNotFoundException();

        }

    }
}
