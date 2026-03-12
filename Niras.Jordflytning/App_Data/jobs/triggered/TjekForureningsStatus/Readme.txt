Dette er et lidt specielt setup, da der er tale om et gammelt projekt som er tilrettet til Azure.
Jobbet skal køres "in place", (se settings.job) da det ellers ikke virker.
Se evt. også https://github.com/projectkudu/kudu/wiki/WebJobs#settingsjob-reference

Filen som kaldes af PowerShell, er Niras.Jordflytning.BatchJob.exe
Konfigurationen til denne, Niras.Jordflytning.BatchJob.exe.config, modificeres runtime, således at Azure Environment variabler tilføjes/ændres. (appSettings, connectionStrings)