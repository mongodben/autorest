# FAQ

## Q: I'm seeing heap out of memory errors, how do I resolve this?

Large OpenAPI specs can cause autorest to require more than the default max memory.
You can increase Node max memory by setting the `--memory` option either in the cli or autorest config file(s).

Example:

```yaml
memory: 8g      # Increase to 8g
memory: 4096m   # Increase to 4g
```

Alternatively setting the `NODE_OPTIONS` environment variable with the `--max_old_space_size=<amount>` flag.

For example

```bash
NODE_OPTIONS=--max_old_space_size=4096 # Increase to 4g
NODE_OPTIONS=--max_old_space_size=8192 # Increast to 8g
```

## Q: I'm running into errors in the [azure-rest-api-specs][azure_rest_api_specs] CI. However, I know I'm doing the right thing. How do I pass CI?

In this case, you want to use suppressions. See docs for them [here][suppressions].

<!-- LINKS -->

[azure_rest_api_specs]: https://github.com/Azure/azure-rest-api-specs
[suppressions]: https://dev.azure.com/azure-sdk/internal/_wiki/wikis/internal.wiki/85/Swagger-Suppression-Process
