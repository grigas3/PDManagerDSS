using System.Collections.Generic;

namespace PDManager.Core.DSS
{
    /// <summary>
    /// Helper methods
    /// </summary>
    public class DSSHelper
    {
        /// <summary>
        /// UPDRS Mapping
        /// </summary>
        /// <returns></returns>
        public static DSSCategoricalValueMappingList BoolToScalePN()
        {
            // default mapping: UPDRS 0,1-->NORMAL, 2-4-->PROBLEMATIC

            return new DSSCategoricalValueMappingList(){
                new DSSCategoricalValueMapping(){
                               Name="Problematic",
                               Value=1
                            },
                             new DSSCategoricalValueMapping(){
                                   Name="Normal",
                               Value=0
                            },
            };
        }

        /// <summary>
        /// UPDRS Mapping
        /// </summary>
        /// <returns></returns>
        public static DSSCategoricalValueMappingList ToScaleSN()
        {
            // default mapping: UPDRS 0,1-->NORMAL, 2-4-->PROBLEMATIC

            return new DSSCategoricalValueMappingList(){
                new DSSCategoricalValueMapping(){
                               Name="severe",
                               Value=2
                            },
                            new DSSCategoricalValueMapping(){
                               Name="moderate",
                               Value=1
                            },
                             new DSSCategoricalValueMapping(){
                                   Name="mild",
                               Value=0
                            },
            };
        }

        /// <summary>
        /// UPDRS Mapping
        /// </summary>
        /// <returns></returns>
        public static List<DSSNumericBin> UPDRSNumericBin()
        {
            // default mapping: UPDRS 0,1-->NORMAL, 2-4-->PROBLEMATIC

            return new List<DSSNumericBin>(){
                new DSSNumericBin(){
                                MinValue=0,
                                MaxValue=2,
                                Value=0,
                                ValueMeaning="UPDRS-0-1"
                            },
                             new DSSNumericBin(){
                                MinValue=2,
                                MaxValue=5,
                                Value=1,
                                ValueMeaning="UPDRS-2-4"
                            },
            };
        }

        /// <summary>
        /// UPDRS Mapping
        /// </summary>
        /// <returns></returns>
        public static List<DSSNumericBin> UPDRSNumericBin2()
        {
            return new List<DSSNumericBin>(){
                new DSSNumericBin(){
                                MinValue=0,
                                MaxValue=1,
                                Value=0,
                                ValueMeaning="UPDRS-0"
                            },
                             new DSSNumericBin(){
                                MinValue=1,
                                MaxValue=2,
                                Value=1,
                                ValueMeaning="UPDRS-1"
                            },
                               new DSSNumericBin(){
                                MinValue=2,
                                MaxValue=3,
                                Value=2,
                                ValueMeaning="UPDRS-2"
                            },
                               new DSSNumericBin(){
                                MinValue=3,
                                MaxValue=4,
                                Value=3,
                                ValueMeaning="UPDRS-3"
                            },
                             new DSSNumericBin(){
                                MinValue=4,
                                MaxValue=5,
                                Value=4,
                                ValueMeaning="UPDRS-4"
                            }
            };
        }
    }
}