# PediatricsChatbot
Chatbot for managing pediatrics appointments

To run:

```
dotnet run --project AppointmentClass

```

To test:

```
dotnet test
```

To test with coverage:

```
dotnet test --collect:"XPlat Code Coverage"
```

Then to make a report:

```
dotnet tool install --global dotnet-reportgenerator-globaltool --version 4.8.13
~/.dotnet/tools/reportgenerator -reports:AppointmentTests\TestResults\221f2836-c4e2-4da4-9dd1-4c1c0ade1496\coverage.cobertura.xml -targetdir:out 
```