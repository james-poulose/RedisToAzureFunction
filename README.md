RedisToAzureFunction
====================
**Goal**
* A CLI to write random objects to Redis.
* An Azure function (Time trigger) which reads from the Redis queue.
* Ability to run and debug Azure Functions locally.

**Get Redis for Windows**
* Get the latest binary from this repo https://github.com/tporadowski/redis.
* Unpack the contents to a folder.
* Run `redis-server.exe` in your console to run the instance. By default the port used is `6379`. You can change that and other default settings by editing `redis.windows.conf`.

**Get Azure Core Tools**
* Download the Azure Core Tools from the Github page (https://github.com/Azure/azure-functions-core-tools) or do a direct download from https://go.microsoft.com/fwlink/?linkid=2135274 and install it. This should make the Azure Core tools available as CLI.
* Test the CLI by running `func --version`. If properly installed, that should give a version number.

**Running**
* Build the solution.
* If not already running, start the Redis server by executing `redis-server.exe` from the Redis root folder.
* Run `R2A.Cli.exe` from the output folder of `RedisToAzureFunction.Cli` project. That will add few objects to the configured queue.
  *  The Redis instance name can be configured in the `App.config` using `Server` key permanantly or be passed in as a parameter with switch `-s` or `--server`.
  *  The Redis queue name can be configured in the `App.config` using `QueueName` key permanantly or be passed in as a parameter with switch `-q` or `--queue`.
* Open a command prompt at the root of `RedisToAzureFunction.Function` project and run command `func start` to start the function.