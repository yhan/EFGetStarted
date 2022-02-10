https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli


dotnet ef migrations add InitialCreate
dotnet ef database update

1. remove previous migration

```cli
dotnet ef migrations remove
```

1. Revert iniial database apply

→ C:\repo\EFGetStarted [main +3 ~3 -2 !]› dotnet ef database update 0
Build started...
Build succeeded.
Reverting migration '20220210222941_InitialCreate'.
Done.