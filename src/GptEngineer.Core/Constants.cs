﻿namespace GptEngineer.Core;

public static class Constants
{
    public const string MONGO_DB = "MongoDb";
    public const string LOG = "logs";
    public const string CONNECTION_STRINGS_OPTIONS = "ConnectionStrings";

    public const string ROOT_COMPONENT = "#app";
    public const string DEFAULT = "default";

    // auth
    public const string AUTHORIZED_CLIENT_NAME = "authorizedClient";
    public const string LOG_IN_PATH = "LogInPath";
    public const string LOG_OUT_PATH = "LogOutPath";
    public const string HEADER_NAME = "X-XSRF-TOKEN";
    public const string COOKIE_NAME = "__Host-X-XSRF-TOKEN";
    public const string XML_HTTP_REQUEST_HEADER = "XMLHttpRequest";
    public const string APPLICATION_JSON = "application/json";

    // logging constants
    public const string VERBOSE = "Verbose";
    public const string DEBUG = "Debug";
    public const string INFORMATION = "Information";
    public const string WARNING = "Warning";
    public const string ERROR = "Error";
    public const string FATAL = "Fatal";

    // configuration
    public const string GPT_OPTIONS = "GptOptions";
    public const string AI_OPTIONS = "AIOptions";
    public const string CERTIFICATE_OPTIONS = "CertificateOptions";
    public const string HOST_OPTIONS = "HostOptions";
    public const string NO_CERTIFICATE_FOUND =
        "CertificateOptions were found however no X509CertificateCollection was found matching the values in the CertificateOptions";
}