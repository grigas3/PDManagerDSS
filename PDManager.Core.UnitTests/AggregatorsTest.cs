using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDManager.Core.Aggregators;
using PDManager.Core.Aggregators.Testing;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.Services.Testing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PDManager.Core.UnitTests
{
    [TestClass]
    public class AggregatorsTest
    {




        [TestMethod]
        public void SaveAggregation()
        {
            DummyAggrDefinitionProvider aggrProvider = new DummyAggrDefinitionProvider();
            var def = aggrProvider.GetConfigFromCode("UPDRS");

            AggrConfig.SaveToFile(def, "updrs.json");

            var defTarget=AggrConfig.LoadFromFile( "updrs.json");

            Assert.AreEqual(def.AggregationType, defTarget.AggregationType);


        }



        /// <summary>
        /// Test ONOFF Total aggregation
        /// </summary>
        /// <returns></returns>

        [TestMethod]
        public async Task TotalOFFTime_Test()
        {

            string patientId = "234234234";

            IDataProxy proxy = new DummyDataProxy(patientId, @".\TestData\symptoms.txt");


            GenericAggregator aggregator = new GenericAggregator(proxy, null, new DummyAggrDefinitionProvider());

            var observations = await aggregator.Run(patientId, "STOFFDUR", null);

            Assert.IsTrue(observations.Count() > 0);
            var mean = observations.Select(e => e.Value).Average();
            Assert.IsTrue(Math.Abs(mean - 4*0.20) < 1, $"OFF Mean time {(mean)}");
                        
        }


        /// <summary>
        /// Test UPDRS Total aggregation
        /// </summary>
        /// <returns></returns>

        [TestMethod]
        public async Task TotalUPDRS_Total_Test()
        {

            string patientId = "234234234";

            IDataProxy proxy = new DummyDataProxy(patientId, @".\TestData\symptoms.txt");

            GenericAggregator aggregator = new GenericAggregator(proxy, null, new DummyAggrDefinitionProvider());

            var observations=await aggregator.Run(patientId, "UPDRSTOTAL", null);

            Assert.IsTrue(observations.Count() > 0);
            var mean = observations.Select(e => e.Value).Average();
            Assert.IsTrue(Math.Abs(mean - 10.8584) < 0.1,$"UPDRS mean score is {mean}");




        }

        /// <summary>
        /// Test UPDRS Day aggregation
        /// </summary>
        /// <returns></returns>

        [TestMethod]
        public async Task TotalUPDRS_DayTest()
        {

            string patientId = "234234234";

            IDataProxy proxy = new DummyDataProxy(patientId, @".\TestData\symptoms.txt");
            

            GenericAggregator aggregator = new GenericAggregator(proxy, null, new DummyAggrDefinitionProvider());

            var observations = await aggregator.Run(patientId, "UPDRSDAY", null);

            Assert.IsTrue(observations.Count() > 0);
            var mean = observations.Select(e => e.Value).Average();
            Assert.IsTrue(Math.Abs(mean - 10.8584) < 0.1, $"UPDRS mean score is {mean}");




        }

        /// <summary>
        /// Test UPDRS Time Aggregation
        /// </summary>
        /// <returns></returns>

        [TestMethod]
        public async Task TotalUPDRS_Time_Test()
        {

            string patientId = "234234234";

            IDataProxy proxy = new DummyDataProxy(patientId, @".\TestData\symptoms.txt");

            GenericAggregator aggregator = new GenericAggregator(proxy, null, new DummyAggrDefinitionProvider());

            var observations = await aggregator.Run(patientId, "UPDRS", null);

            Assert.IsTrue(observations.Count() > 0);
            var mean = observations.Select(e => e.Value).Average();
            Assert.IsTrue(Math.Abs(mean - 10.8584) < 0.1, $"UPDRS mean score is {mean}");

        }

        /// <summary>
        /// Test UPDRS Time Aggregation
        /// </summary>
        /// <returns></returns>

        [TestMethod]
        public async Task TotalUPDRS_Max_Test()
        {

            string patientId = "234234234";

            IDataProxy proxy = new DummyDataProxy(patientId, @".\TestData\symptoms.txt");


            GenericAggregator aggregator = new GenericAggregator(proxy, null, new DummyAggrDefinitionProvider());

            var observations = await aggregator.Run(patientId, "UPDRS", null,null,"max");

            Assert.IsTrue(observations.Count() ==1);
            var max = observations.Select(e => e.Value).Max();
            Assert.IsTrue(Math.Abs(max - 18.4114) < 0.1, $"UPDRS max score is {max}");

        }



        /// <summary>
        /// Mean Fluctuation Index Aggregation
        /// </summary>
        /// <returns></returns>

        [TestMethod]
        public async Task MFI_Test()
        {

            string patientId = "234234234";
            IDataProxy proxy = new DummyDataProxy(patientId, @".\TestData\symptoms.txt");


            GenericAggregator aggregator = new GenericAggregator(proxy, null, new DummyAggrDefinitionProvider());

            //var mfiObservation = await aggregator.Run(patientId, "STFLUCT", null, null, null);
            var mfiObservation = await aggregator.Run(patientId, "UPDRS", null, null, "mfi");



            Assert.IsTrue(mfiObservation.Count() == 1);
       
            var mfi = mfiObservation.Select(e => e.Value).Average();
          
            Assert.IsTrue(Math.Abs((mfi) - 7.552975) < 0.1, $"MFI score is {mfi}");

        }


        /// <summary>
        /// Mean Fluctuation Index Aggregation
        /// </summary>
        /// <returns></returns>

        [TestMethod]
        public async Task MFI_Test2()
        {

            string patientId = "234234234";
            IDataProxy proxy = new DummyDataProxy(patientId, @".\TestData\symptoms.txt");


            GenericAggregator aggregator = new GenericAggregator(proxy, null, new DummyAggrDefinitionProvider());

            var mfiObservation = await aggregator.Run(patientId, "STFLUCT", null, null, null);
            //var mfiObservation = await aggregator.Run(patientId, "UPDRS", null, null, "mfi");



            Assert.IsTrue(mfiObservation.Count() == 1);

            var mfi = mfiObservation.Select(e => e.Value).Average();

            Assert.IsTrue(Math.Abs((mfi) - 7.552975) < 0.1, $"MFI score is {mfi}");

        }



    }
}
