# HotelAvailability
* .Net core api to call external api for assessment.
* Api is getting hotel's list for external api throgh HttpClient
* If hotel RateType from external response is PerNight then it will calculate the price for all input nights
* InMemory Cache is used to reduce the response time of api call, instead of sending call to external api all the time
* Run api, provide input paramenters LocationId, and NumberOfNights and execute method
* Error are in Logs folder

## Layers:
* Api layer REST api (Visual studio template for Api)
* Api Layer will check cache if cache exist for request it will return response to user, else it will send call to core layer and save response in cahce and return result.
* Core layer is for Communicating with external Api and get and process data and return it back to api

##NOTE##

## How To Run:
* Download or clone repository
* Required .Net Core framework is .Net core 2.2
* Open solution in visual studio
* Check and change confurations in AppSetting
* Restore packages and run

## Technologies & frameworks stack:
* Visual Studio 2017
* .Net core 2.2
* RESTful Api
* HttpClient
* InMemoryCache

## Supports:

* Swagger for documentation and api testing
* Logging in file
* Serilog
* Configurable Cache and Some external api settings 
