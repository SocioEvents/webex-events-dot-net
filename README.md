[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE.txt)
[![Webex Events](https://github.com/SocioEvents/webex-events-dot-net-sdk/actions/workflows/test.yml/badge.svg)](https://github.com/SocioEvents/webex-events-dot-net-sdk/actions)

⚠️ This library has not been released yet.
# Webex Events Api .NET SDK

Webex Events provides a range of additional SDKs to accelerate your development process.
They allow a standardized way for developers to interact with and leverage the features and functionalities.
Pre-built code modules will help access the APIs with your private keys, simplifying data gathering and update flows.

Requirements
-----------------

.NET 8+

Installation
-----------------
TODO:

Configuration
-----------------

TODO:

Usage
-----------------

TODO:

By default some HTTP statuses are retriable such as `408, 409, 429, 502, 503, 504`. This library tries this status
codes 5 times by default. If this is not sufficient, increase max retry count through Configuration class or re-catch
the exceptions to implement your logic here.

For Introspection Query
-----------------
TODO:

Idempotency
-----------------
The API supports idempotency for safely retrying requests without accidentally performing the same operation twice.
When doing a mutation request, use an idempotency key. If a connection error occurs, you can repeat
the request without risk of creating a second object or performing the update twice.

To perform mutation request, you must add a header which contains the idempotency key such as
`Idempotency-Key: <your key>`. The SDK does not produce an Idempotency Key on behalf of you if it is missed.
The SDK also validates the key on runtime, if it is not valid UUID token it will raise an exception. Here is an example
like the following:

TODO:

Telemetry Data Collection
-----------------
Webex Events collects telemetry data, including hostname, operating system, language and SDK version, via API requests.
This information allows us to improve our services and track any usage-related faults/issues. We handle all data with
the utmost respect for your privacy. For more details, please refer to the Privacy Policy at https://www.cisco.com/c/en/us/about/legal/privacy-full.html

Development
-----------------

After checking out the repo, install dependencies. Then run tests.

Contributing
-----------------
Please see the [contributing guidelines](CONTRIBUTING.md).

License
-----------------

The library is available as open source under the terms of the [MIT License](https://opensource.org/licenses/MIT).

Code of Conduct
-----------------

Everyone interacting in the Webex Events API project's codebases, issue trackers, chat rooms and mailing lists is expected to follow the [code of conduct](https://github.com/SocioEvents/webex-events-dot-net-sdk/blob/main/CODE_OF_CONDUCT.md).
