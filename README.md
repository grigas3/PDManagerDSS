
# PDManager DSS
This is a test web site in ASP .NET Core in order to test DSS and Aggregation functionalities.
Technologies used to develop this site:
* [ASP.NET Core](https://get.asp.net/)
* [C# for a cross-platform server-side code](https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx)
* [Typescript TypeScript for client-side code](https://angular.io/)
* [Webpack for building and bundling client-side resources](https://webpack.github.io/)
* [Bootstrap](http://getbootstrap.com/) for layout and styling
* [Json.Net](https://www.newtonsoft.com/json) Popular high-performance JSON framework for .NET
* [HangFire](https://www.hangfire.io/) An easy way to perform background processing in .NET and .NET Core applications. No
* [Swagger](https://swagger.io/). Swagger is the world’s largest framework of API developer tools for the OpenAPI Specification(OAS), enabling development across the entire API lifecycle, from design and documentation, to test and deployment.Windows Service or separate process required. The [Swashbuckle.AspNetCore]|(https://github.com/domaindrivendev/Swashbuckle.AspNetCore) implementation is used for ASP.NET Core.

## The PDManager DSS architecture
The following figure depicts the UML component diagram of the PDManager DSS
[![N|Solid](https://drive.google.com/uc?id=1Ewr9rELNT2w6mrA0E-thJDGG4TyF6ZAJ)](https://drive.google.com/uc?id=1Ewr9rELNT2w6mrA0E-thJDGG4TyF6ZAJ)

The main modules/services of the PDManager DSS are 
* Data Proxy. The Data Proxy provides access to PDManager Observations (add link to wiki) and Patient Clinical data. The access is through a local repository or to a remote repository via REST services. The Data Proxy exposes the IDataProxy interface with a number of basic data insert/fetch methdos.
* Aggregator 
* 


# Aggregation
The PDManager has a repository of observations regarding motor and non-motor symptoms related to PD. However, the DSS input requires a post-process of those "raw" observations. In order to fullfil the requirements of the DSS the following aggregation schema is proposed

[![Aggregation Flow](https://drive.google.com/uc?id=1BNYjS80Iz7uRGAOETfqP8Vs40ua75oyk)](https://drive.google.com/uc?id=1BNYjS80Iz7uRGAOETfqP8Vs40ua75oyk)
* 1st Level aggregation. Data are gathered usign the IDataProxy service and aggregated with a linear regression function defined in the aggregation definition JSON file.
* Filtering. In this step (if defined) the 1st level aggregation output is thresholded. A number of different threshold types are allowed (ADD WIKI).
* 2nd Level Aggregation. The second level aggregation allows the aggregation of the output of the filtering.


## Aggregation Definition
The aggregation definition is a json object with the following schema
```
{
	"definitions": {
		"AggrVariable": {
			"type": [
				"object",
				"null"
			],
			"properties": {
				"Source": {
					"description": "Source of the variable the source can be 1) observation and 2) clinical",
					"type": "string"
				},
				"Code": {
					"description": "Variable code",
					"type": "string"
				},
				"Weight": {
					"description": "Variable weight. This is the A in the Ax+B regression function.",
					"type": "number"
				}
			},
			"required": [
				"Source",
				"Code",
				"Weight"
			]
		}
	},
	"type": "object",
	"properties": {
		"AggregationType": {
			"description": "Aggregation Type. Possible values 1) time: Aggregate observations per time of day, 2) day: Aggregate observation per day, 3) total: Aggregation of all values",
			"type": "string"
		},
		"MetaAggregationType": {
			"description": "Meta Aggregation Type. Meta Aggregation occurs after aggregation on raw observations and filtering. Possible values 1) sum: Sum of observations 2) average: Average of observations, 3) std: Std of observations, 4) max: max of observations 5) min: min of observations, 6) mfi: Mean Fluctuation Index, 7) cv: Coefficient of variation, 8) count: count number of observations, 9) none: Get all metaobservations ",
			"type": "string"
		},
		"MetaScale": {
			"description": "Scale meta aggregated value",
			"type": "number",
			"default": 1
		},
		"Variables": {
			"type": "array",
			"items": {
				"$ref": "#/definitions/AggrVariable"
			}
		},
		"Beta": {
			"description": "Linear Aggregation is a linear regression of the form A*X+B where X the input variables. The Beta property corresponse to the B of the linear regression function",
			"type": "number"
		},
		"ThresholdValue": {
			"description": "Threshold Value applied in the filter step of the aggregation",
			"type": "number"
		},
		"ThresholdType": {
			"description": "Threshold type applied on filter step of aggregation",
			"type": [
				"string",
				"null"
			]
		},
		"Threshold": {
			"description": "Use a thresholding in filter step.",
			"type": "boolean"
		},
		"Code": {
			"description": "Aggregated observation code",
			"type": "string"
		},
		"Name": {
			"description": "Name of the aggregation variable",
			"type": "string"
		},
		"Description": {
			"type": [
				"string",
				"null"
			]
		},
		"Version": {
			"description": "Version of the aggregation definition",
			"type": "string"
		}
	},
	"required": [
		"AggregationType",
		"MetaAggregationType",
		"MetaScale",
		"Variables",
		"Beta",
		"ThresholdValue",
		"ThresholdType",
		"Threshold",
		"Code",
		"Name",
		"Description",
		"Version"
	]
}

```

## Aggregation Example

An example for the UPDRS score (ref to Wiki) is to aggregate a number of UPDRS item variables per time of day. Then a total UPDRS score per time of day can be extracted. This aggregation is also the basis for wearing off estimation and patient's fluctuation score. The Json for the UPDRS score is presented in the json below. The codes STBRAD30 etc. are the observation codes defined in the PDManager observation dictionary.

```
{
	"AggregationType": "time",
	"MetaAggregationType": null,
	"MetaScale": 1.0,
	"Variables": [{
			"Source": "observation",
			"Code": "STBRAD30",
			"Weight": 8.2422
		}, {
			"Source": "observation",
			"Code": "STUPDRSG",
			"Weight": 4.763
		}, {
			"Source": "observation",
			"Code": "STFOG",
			"Weight": 4.0556
		}, {
			"Source": "observation",
			"Code": "STTRMA30",
			"Weight": 1.49
		}, {
			"Source": "observation",
			"Code": "STTRMC30",
			"Weight": 2.5942
		}, {
			"Source": "observation",
			"Code": "STDYS30",
			"Weight": 2.7079
		}
	],
	"Beta": 3.5621,
	"ThresholdValue": 0.0,
	"ThresholdType": null,
	"Threshold": false,
	"Code": "UPDRS",
	"Name": "UPDRS",
	"Description": "UPDRS PDManager Score",
	"Version": "1.0"
}

```





# DSS
The DSS takes as input "raw" observations aggregated over a period of tim (this functionality is already implemented by the REST API of the PDManager), patient's clinical information and aggregated observations (meta-observations).
The DSS is based on the [DEXI](http://kt.ijs.si/MarkoBohanec/dexi.html) model to provide an output given the specific inputs. In order to be able to generalize and adjust the input to a new DEXI model a JSON definition of the DSS is proposed. Using this definition any DEXI input can be fed with the required input from the PDManager repository without the need of changing the code.
The DSS Definition is according to the following schema

```
{
	"definitions": {
		"DSSCategoricalValueMapping": {
			"type": [
				"object",
				"null"
			],
			"properties": {
				"Name": {
					"type": [
						"string",
						"null"
					]
				},
				"Value": {
					"type": "integer"
				},
				"ValueMeaning": {
					"type": [
						"string",
						"null"
					]
				}
			},
			"required": [
				"Name",
				"Value",
				"ValueMeaning"
			]
		},
		"DSSNumericBin": {
			"type": [
				"object",
				"null"
			],
			"properties": {
				"MinValue": {
					"type": "number"
				},
				"MaxValue": {
					"type": "number"
				},
				"Value": {
					"type": "integer"
				},
				"ValueMeaning": {
					"type": [
						"string",
						"null"
					]
				}
			},
			"required": [
				"MinValue",
				"MaxValue",
				"Value",
				"ValueMeaning"
			]
		},
		"DSSNumericMapping": {
			"description": "NumericMapping is used to scale and translate if required the original numeric value BEFORE are use to map continuous values to specific DEXI discrete input values",
			"type": [
				"object",
				"null"
			],
			"properties": {
				"Scale": {
					"type": "number"
				},
				"Bias": {
					"type": "number"
				}
			},
			"required": [
				"Scale",
				"Bias"
			]
		},
		"DSSValueMapping": {
			"type": [
				"object",
				"null"
			],
			"properties": {
				"Name": {
					"description": "name",
					"type": [
						"string",
						"null"
					]
				},
				"Code": {
					"description": "Code",
					"type": "string"
				},
				"DefaultValue": {
					"description": "Default Value. This value is used if the attribute is not available in the repository",
					"type": "integer"
				},
				"Source": {
					"description": "Source of attribute. The possible values are 1) observation and 2) clinical ",
					"type": "string"
				},
				"ValueType": {
					"description": "Value Type. Possible values are 'numeric' for numeric attributes 'categorical' for categorical attributes",
					"type": "string"
				},
				"CategoryMapping": {
					"description": "Category mapping for categorical values. It maps the original value to a DEXI model one",
					"type": [
						"array",
						"null"
					],
					"items": {
						"$ref": "#/definitions/DSSCategoricalValueMapping"
					}
				},
				"NumericBins": {
					"description": "Numeric bins are use to map continuous values to specific DEXI discrete input values",
					"type": [
						"array",
						"null"
					],
					"items": {
						"$ref": "#/definitions/DSSNumericBin"
					}
				},
				"NumericMapping": {
					"$ref": "#/definitions/DSSNumericMapping"
				},
				"Numeric": {
					"type": "boolean"
				}
			},
			"required": [
				"Name",
				"Code",
				"DefaultValue",
				"Source",
				"ValueType",
				"CategoryMapping",
				"NumericBins",
				"NumericMapping",
				"Numeric"
			]
		}
	},
	"type": "object",
	"properties": {
		"Name": {
			"description": "DSS name",
			"type": "string"
		},
		"Version": {
			"description": "Version of the DSS Model",
			"type": "string"
		},
		"DexiFile": {
			"description": "Dexi File Reference",
			"type": "string"
		},
		"Input": {
			"description": "Input",
			"type": [
				"array",
				"null"
			],
			"items": {
				"$ref": "#/definitions/DSSValueMapping"
			}
		},
		"AggregationPeriodDays": {
			"description": "Aggregation Period Days(Default 30)",
			"type": "integer"
		}
	},
	"required": [
		"Name",
		"Version",
		"DexiFile",
		"Input",
		"AggregationPeriodDays"
	]
}
```


# This demo implementation
The demo implementation uses EF context in order to store temporarily (uses memory storage) the Aggregation and DSS entities. It also uses HangFire in order to implement a simple job scheduling to run alert evaluation job.



# Prerequisites
* VS2017
* ASP. NET Core
* 




# Todos
 - More Tests
 - Test Notifications
 - Integrate with PDManager Application
 - Logging


License
----

MIT


