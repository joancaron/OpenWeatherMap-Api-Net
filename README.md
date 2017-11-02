OpenWeatherMap-Api-Net
======================

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/6924c4f0ffe04b3cbeeff87a718a494d)](https://www.codacy.com/app/joancaron/OpenWeatherMap-Api-Net?utm_source=github.com&utm_medium=referral&utm_content=joancaron/OpenWeatherMap-Api-Net&utm_campaign=badger)

This project is a fully asynchronous .NET library for interacting with the great [OpenWeatherMap API](http://openweathermap.org/API).

## Usage examples

```c#
var client = new OpenWeatherMapClient("optionalAppId");
var currentWeather = await client.CurrentWeather.GetByName("London");
Console.WriteLine(currentWeather.Weather.Value);
```

### Supported Platforms

* .NET Standard 1.1

### Copyright and License

Copyright 2014 Joan Caron

Licensed under the MIT License
