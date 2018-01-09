# UngracefulUnloadOfFunctions
This is a quick repro to show how Azure Functions do not shut down gracefully but rather kill any actively running functions when stopped, restarted, deployed, or deployment slots are swapped.
