# BudoHub.Core

<img src="https://teamcity.brogrammer.life/app/rest/builds/aggregated/strob:(buildType:(project:(BudoHubCore)))/statusIcon" />

# Configuration


All top-level options objects should now end it “options”. This is an attempt to try to make bad json mappings easier to spot.

Notable exception is the ApplicationSecrets configuration object which is only used by the vault client to create a default “Secrets” json file. This object should never be used outside of the core libraries.

**S** \= string, **C** \= collection type, **B** = boolean, **I** \= integer, **L** \= long, **T** = custom type

# Top Level Configuration Options

## ApplicationInformationOptions - required

**Type** | **Property Name** | **Default Value** | **Required**

**S** | ApplicationName | *Should match lowered assembly name* | true |

**S** | ApplicationDescription | ManageAmerica Application | false |

**S** | ApplicationBaseUrl | "[http://locahost:7000"](http://locahost:7000%22 "http://locahost:7000%22") | true |

**S** | Environment | Development | true |

**B**

 |

UseKestrel

 |

true

 |

true

 |
|

**B**

 |

CaptureStartupErrors

 |

true

 |

false

 |
|

**B**

 |

EnableIntrospection

 |

true

 |

false

 |
|

**C**

 |

SupportedCultures

 |

string collection of supported cultures

 |

false

 |

## ApplicationPipelineOptions - required

Handles the registration of various middleware pipeline objects. The registration of these objects can be found in the core libraries at: *ManageAmerica.Core/src/ManageAmerica.Core.AspnetCore.Extensions/MiddlewarePipeline/*

|

 |

**Property Name**

 |

**Purpose**

 |

**Default Value**

 |
|

**B**

 |

UseEndPoints

 |

If not enabled, no external endpoints will be available for the service. Consumers are a use case where endpoints are not needed.

 |

true

 |
|

**B**

 |

UseRouting

 |

Enables Aspnet Core routing.

 |

true

 |
|

**B**

 |

UseStaticFiles

 |

Allow service js, html, images from wwwroot

 |

true

 |
|

**B**

 |

UseDefaultFiles

 |

If true then default http files will be searched in the www folders for index.html, ect.

 |

true

 |
|

**B**

 |

UseHttpsRedirection

 |

In kubernetes we terminate the cert at the load balancer, so this should be false for any service running in kubernetes.

 |

false

 |
|

**B**

 |

EnableHealthChecks

 |

If disabled, kubernetes ingress services will not come online.

 |

true

 |
|

**B**

 |

UsePolly

 |

For use by child solutions to determine if polly should be used.

 |

true

 |
|

**B**

 |

UseSwagger

 |

Only works in non-production builds.

 |

true

 |
|

**B**

 |

EnableSignalR

 |

Set to true to enable signalR hubs.

 |

false

 |
|

**B**

 |

ForwardHeaders

 |

Don’t change unless you know what you are doing.

 |

true

 |
|

**B**

 |

ForwardOnlyXForwardHeaders

 |

Don’t change unless you know what you are doing.

 |

true

 |
|

**B**

 |

UseIdsrvProxyFix

 |

This is for IDSRV only and should remain false.

 |

false

 |
|

**B**

 |

DebugForwardedHeaders

 |

Currently not being used.

 |

false

 |
|

**B**

 |

EnableMetrics

 |

Enables prometheus metrics collection and expose at /metrics. Is independent of the “UseEndpoints” configuration item.

 |

true

 |
|

**B**

 |

UseHsts

 |

Forces the browser to connect to the application using https requests.

 |

true

 |
|

**B**

 |

UseSerilogRequestLogging

 |

Enables logging of all http traffic to Elastic.

 |

true

 |
|

**B**

 |

WriteLogsToFile

 |

When enabled, creates a local logs folder and writes logs to a rolling log file.

 |

false

 |
|

**B**

 |

AddMvcControllers

 |

Enables MVC controller stack, including routing.

 |

true

 |
|

**B**

 |

MapRazorPages

 |

Enables MVC Razor pages, including routing.

 |

false

 |

## KestrelOptions - required

|

 |

**Property Name**

 |

**Purpose**

 |

**Default**

 |

**Required**

 |
|

**I**

 |

HttpPort

 |

The non-ssl port the services listens on.

 |

7000

 |

true

 |
|

**I**

 |

HttpsPort

 |

The ssl port the service listens on.

 |

7001

 |

 |

## CorsOptions

|

 |

**Property Name**

 |

**Purpose**

 |

**Default**

 |

**Required**

 |
|

**I**

 |

Headers

 |

Apart from the headers automatically set by the user agent (for example, `Connection`, `User-Agent`, or [the other headers defined in the Fetch spec as a *forbidden header name*](https://fetch.spec.whatwg.org/#forbidden-header-name "https://fetch.spec.whatwg.org/#forbidden-header-name")), the only headers which are allowed to be manually set are [those which the Fetch spec defines as a CORS-safelisted request-header](https://fetch.spec.whatwg.org/#cors-safelisted-request-header "https://fetch.spec.whatwg.org/#cors-safelisted-request-header"), which are:

*   `Accept`

    *   `Accept-Language`

    *   `Content-Language`

    *   `Content-Type` (please note the additional requirements below)

    *   `Range` (only with a [simple range header value](https://fetch.spec.whatwg.org/#simple-range-header-value "https://fetch.spec.whatwg.org/#simple-range-header-value"); e.g., `bytes=256-` or `bytes=127-255`)

 |

empty collection

 |

false

 |
|

**I**

 |

Methods

 |

A collection of allowed cors http verbs

GET, POST, HEAD

 |

empty collection

 |

false

 |
|

**C**

 |

Origins

 |

A collection of valid absolute, top-level uri’s that are allowed to access the services via cors calls

 |

empty collection

 |

true

 |

## JsonOptions

|

 |

**Property Name**

 |

**Purpose**

 |

**Default**

 |
|

**B**

 |

AllowTrailingCommas

 |

 |

true

 |
|

**B**

 |

IgnoreNullValues

 |

 |

true

 |
|

**B**

 |

IgnoreReadOnlyProperties

 |

 |

true

 |
|

**B**

 |

IgnoreReadOnlyFields

 |

 |

true

 |
|

**B**

 |

IncludeFields

 |

 |

false

 |
|

**B**

 |

PropertyNameCaseInsensitive

 |

 |

true

 |
|

**B**

 |

WriteIndented

 |

 |

false

 |

## MinimalApiEndpointsOptions

|

 |

**Property Name**

 |

**Purpose**

 |

**Default**

 |

**Required**

 |
|

**T**

 |

[Uptime](https://manageamerica.atlassian.net/wiki/spaces/~450060327/pages/1490518031/Core+13.x+Configuration#%5BinlineExtension%5DMinimalApiEndpoint "https://manageamerica.atlassian.net/wiki/spaces/~450060327/pages/1490518031/Core+13.x+Configuration#%5BinlineExtension%5DMinimalApiEndpoint")

 |

Sets the values to be used for an uptime endpoint

 |

NULL

 |

true

 |

## SecurityOptions - required

This configuration object should live separate from the mundane AppSettings files. Sensitive information is stored here.

|

 |

**Property Name**

 |

**Purpose**

 |

**Default**

 |

**Required**

 |
|

**B**

 |

UseCoreCookies

 |

When true, registers the Aspnet Core cookie provider as the default authentication scheme.

 |

false

 |

false

 |
|

**B**

 |

UseAuthorization

 |

Is actually called by Aspnet by default, only included here since folks are use to this and the UseAuthentication extension to be called hand in hand. It’s a warm fuzzy

 |

true

 |

false

 |
|

**B**

 |

UseAuthentication

 |

If false then no default authentication is registered in the service collection. Additionally, the default fallback policy is not enabled.

 |

true

 |

false

 |
|

**B**

 |

DisableFallbackPolicy

 |

If set to true will disable the default authorize required policy.

 |

false

 |

false

 |
|

**T**

 |

[Jwt](https://manageamerica.atlassian.net/wiki/spaces/~450060327/pages/1490518031/Core+13.x+Configuration#%5BinlineExtension%5DJWT "https://manageamerica.atlassian.net/wiki/spaces/~450060327/pages/1490518031/Core+13.x+Configuration#%5BinlineExtension%5DJWT")

 |

Defines Jwt options.

 |

default()

 |

true

 |
|

**T**

 |

[IdentityInformation](https://manageamerica.atlassian.net/wiki/spaces/~450060327/pages/1490518031/Core+13.x+Configuration#%5BinlineExtension%5DIdentityInformation "https://manageamerica.atlassian.net/wiki/spaces/~450060327/pages/1490518031/Core+13.x+Configuration#%5BinlineExtension%5DIdentityInformation")

 |

Defines a client secret and client id for use with OIDC systems, especially client-credentials type flows.

 |

default()

 |

false

 |
|

**T**

 |

[CookieOptions](https://manageamerica.atlassian.net/wiki/spaces/~450060327/pages/1490518031/Core+13.x+Configuration#%5BinlineExtension%5DCookieOptions "https://manageamerica.atlassian.net/wiki/spaces/~450060327/pages/1490518031/Core+13.x+Configuration#%5BinlineExtension%5DCookieOptions")

 |

Defines various cookie options.

 |

default()

 |

false

 |
|

**T**

 |

PolicyOptions

 |

Custom Policy Options type

[https://github.com/ma-dev-global/ManageAmerica.Core/blob/main/src/ManageAmerica.Core.AspnetCore.Abstractions/Configuration/PolicyOption.cs](https://github.com/ma-dev-global/ManageAmerica.Core/blob/main/src/ManageAmerica.Core.AspnetCore.Abstractions/Configuration/PolicyOption.cs "https://github.com/ma-dev-global/ManageAmerica.Core/blob/main/src/ManageAmerica.Core.AspnetCore.Abstractions/Configuration/PolicyOption.cs")

 |

default()

 |

false

 |

## ConnectionStrings

Connection strings need to be registered inside child services, the connection string name is set by that system and must then be referenced a such.

i.e. the following json snippet:

"ConnectionStrings": { "DefaultConnectionString": "mongodbxxxxxxxxxxxretryWrites=true&w=majority&tls=true" }

Would have to be configured with a Key of “DefaultConnectionString” in the child project using it.

|

**S**

 |

Connection string name (dynamic)

 |

Connection string value

 |
|

**S?**

 |

Connection string name (dynamic)

 |

Connection string value

 |
|

**S?**

 |

Connection string name (dynamic)

 |

Connection string value

 |

## SwaggerOptions

|

 |

**Property Name**

 |

**Purpose**

 |

**Default**

 |
|

**B**

 |

Enabled

 |

 |

true

 |
|

**S**

 |

Theme

 |

Enables selecting a default custom theme for swagger

 |

ManageAmerica

 |
|

**T**

 |

Client

 |

Holds client secret, client id and Scopes

 |

 |

## ElasticOptions

|

 |

**Property Name**

 |

**Purpose**

 |

**Default**

 |
|

**S?**

 |

Url

 |

Absolute uri to the elasticsearch service.

 |

NULL

 |
|

**S?**

 |

UserName

 |

Elasticsearch username

 |

NULL

 |
|

**S?**

 |

Password

 |

Elasticsearch password

 |

NULL

 |
|

**S?**

 |

Index

 |

Index to which to write

 |

NULL

 |

## CertificateOptions

These options should not be in production. All certificate options are handled by the load balancer in kubernetes. Additionally, most services you will debug locally do not need these options either. Only use if you are sure what you are doing.

|

 |

**Property Name**

 |

**Purpose**

 |

**Default**

 |
|

**B**

 |

UseWindowsStore

 |

 |

false

 |
|

**S?**

 |

CertificateFileLocation

 |

 |

NULL

 |
|

**S?**

 |

CertificateFileName

 |

 |

NULL

 |
|

**S?**

 |

CertificatePassword

 |

 |

NULL

 |
|

**S?**

 |

CertificateHost

 |

 |

NULL

 |
|

**S?**

 |

CertificateStoreLocation

 |

 |

NULL

 |
|

**S?**

 |

Issuer

 |

 |

NULL

 |
|

**S?**

 |

SerialNumber

 |

 |

NULL

 |
|

**S?**

 |

Subject

 |

 |

NULL

 |
|

**S?**

 |

Thumbprint

 |

 |

NULL

 |
|

**S?**

 |

Version

 |

 |

NULL

 |

# Embedded Configuration Options

### JWT

|

 |

**Property Name**

 |

**Purpose**

 |

**Default**

 |
|

**S**

 |

ClaimsIssuer

 |

Identifies what services issued a given token

 |

[identity.manageamerica.com](http://identity.manageamerica.com "http://identity.manageamerica.com")

 |
|

**S**

 |

Audience

 |

To whom the token is valid

 |

NULL

 |
|

**L**

 |

RefreshIntervalInMS

 |

the minimum time between retrievals, in the event that a retrieval failed, or that a refresh was explicitly requested.

 |

60000, 20 minutes

 |
|

**B**

 |

SaveToken

 |

Defines whether the bearer token should be stored in the AuthenticationProperties after a successful authorization.

 |

true

 |
|

**B**

 |

IncludeErrorDetails

 |

Defines whether the token validation errors should be returned to the caller. Enabled by default, this option can be disabled to prevent the JWT handler from returning an error and an error\_description in the WWW-Authenticate header.

 |

false

 |
|

**B**

 |

RequireHttpsMetadata

 |

Gets or sets if HTTPS is required for the metadata address or authority. The default is true. This should be disabled only in development environments.

 |

false

 |
|

**S**

 |

Authority

 |

Authority to use when making OpenIdConnect calls.

 |

"[https://identity.manageamerica.com"](https://identity.manageamerica.com%22 "https://identity.manageamerica.com%22")

 |
|

**B**

 |

RequireSignedTokens

 |

Adds RequireSignedTokens with value as TokenValidationParameters

 |

true

 |
|

**B**

 |

ValidateAudience

 |

Adds validate audience to the TokenValidationParameters

 |

false

 |
|

**B**

 |

ValidateIssuer

 |

Adds validate issuer to the TokenValidationParameters

 |

false

 |
|

**B**

 |

Enabled

 |

If false, no jwt bearer support added to service.

 |

true

 |
|

**B**

 |

SetAsDefaultAuthentication

 |

If true, Jwt bearer scheme added as default authentication scheme

 |

true

 |
|

**B**

 |

EnableIntrospection

 |

If true adds introspection endpoints to the service.

 |

false

 |
|

**S?**

 |

ClientId

 |

OIDC client id for this JWT

 |

NULL

 |
|

**S?**

 |

ClientSecret

 |

OIDC client secret for this JWT

 |

NULL

 |
|

**C**

 |

ValidAudiences

 |

Collection of audiences this JWT is valid from

 |

empty collection

 |

### IdentityInformation

|

 |

**Property Name**

 |

**Purpose**

 |

**Default**

 |
|

**S?**

 |

ClientId

 |

OIDC client id for this JWT

 |

NULL

 |
|

**S?**

 |

ClientSecret

 |

OIDC client secret for this JWT

 |

NULL

 |

### CookieOptions

|

 |

**Property Name**

 |

**Purpose**

 |

**Default**

 |
|

**S?**

 |

CookieSchemeName

 |

The value passed to AddAuthentication as the cookie scheme name to use.

 |

Cookies

 |
|

**B**

 |

IsDefaultScheme

 |

If trues sets the cookie scheme name as the default authentication scheme.

 |

false

 |
|

**S**

 |

LoginPath

 |

Informs the middleware that it should change an outgoing 401 Unauthorized status code into a 302 redirection onto the given login path. The current url which generated the 401 is added to the LoginPath as a query string parameter named by the ReturnUrlParameter. Once a request to the LoginPath grants a new SignIn identity, the ReturnUrlParameter value is used to redirect the browser back
to the url which caused the original unauthorized status code.

 |

/account/c-login

 |
|

**S**

 |

AccessDeniedPath

 |

Relative path to 403 page

 |

/account/forbidden/

 |
|

**S**

 |

LogoutPath

 |

If the LogoutPath is provided the middleware then a request to that path will redirect based on the ReturnUrlParameter.

 |

/

 |
|

**S**

 |

ReturnUrlParameter

 |

Determines the name of the query string parameter which is appended by the middleware when a 401 Unauthorized status code is changed to a 302 redirect onto the login path. This is also the query string parameter looked for when a request arrives on the login path or logout path, in order to return to the original url after the action is performed.

 |

NULL

 |
|

**S?**

 |

ForwardChallenge

 |

If set, this specifies the target scheme that this scheme should forward ChallengeAsync calls to.For example

`1Context.ChallengeAsync("ThisScheme") => 2Context.ChallengeAsync("ForwardChallengeValue");`

Set the target to the current scheme to disable forwarding and allow normal processing.

 |

NULL

 |
|

**S?**

 |

ForwardDefault

 |

If set, this specifies the target scheme that this scheme should forward AuthenticateAsync calls to.
For example

`1Context.AuthenticateAsync("ThisScheme") => 2Context.AuthenticateAsync("ForwardAuthenticateValue");`

Set the target to the current scheme to disable forwarding and allow normal processing.

 |

NULL

 |
|

**S?**

 |

ForwardForbid

 |

If set, this specifies the target scheme that this scheme should forward SignInAsync calls to.
For example

`1Context.SignInAsync("ThisScheme") => 2Context.SignInAsync("ForwardSignInValue");`

Set the target to the current scheme to disable forwarding and allow normal processing.

 |

NULL

 |
|

**S?**

 |

ForwardSignIn

 |

If set, this specifies the target scheme that this scheme should forward SignOutAsync calls to.
For example

`1Context.SignOutAsync("ThisScheme") => 2Context.SignOutAsync("ForwardSignOutValue");`

Set the target to the current scheme to disable forwarding and allow normal processing.

 |

NULL

 |
|

**S?**

 |

ClaimsIssuer

 |

The issuer that should be used for any claims that are created

 |

NULL

 |
|

**B**

 |

SlidingExpiration

 |

If set allows re-issuing a cookie if the request falls within halfway through the expiration time.

 |

true

 |

### MinimalApiEndpoint

|

 |

**Property Name**

 |

**Purpose**

 |

**Default**

 |
|

**S?**

 |

Route

 |

Relative uri which resolves to the uptime endpoint.

 |

/uptime

 |
|

**B**

 |

Enabled

 |

 |

true

 |
|

**B**

 |

RequireAuthentication

 |

 |

false

 |
