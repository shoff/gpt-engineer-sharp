namespace GptEngineer.Core;

public static class Constants
{
    public const string MONGO_DB = "MongoDb";
    public const string LOG = "logs";
    public const string CONNECTION_STRINGS_OPTIONS = "ConnectionStrings";

    public const string ROOT_COMPONENT = "#app";
    public const string DEFAULT = "default";

    public const string ROLE = "role";
    public const string USER = "user";
    public const string ASSISTANT = "assistant";
    public const string SYSTEM = "system";

    // steps
    public const string SPECIFICATION = "specification";
    public const string MAIN_PROMPT = "main_prompt";
    public const string SPEC = "spec";
    public const string CONTENT = "content";
    public const string RESPEC = "respec";
    public const string UNIT_TESTS = "unit_tests";
    public const string USE_QA = "use_qa";
    public const string CLARIFY = "clarify";
    public const string KEY = "Key";
    
    public const string AI_MEMORY_OPTIONS = "AIMemoryOptions";
    public const string INPUT_OPTIONS = "InputOptions";
    public const string STEP_OPTIONS = "StepOptions";
    public const string SPECIFICATION_STORE_OPTIONS = "SpecificationStoreOptions";
    public const string PRE_PROMPT_OPTIONS = "PrePromptOptions";
    public const string REVIEW_OPTIONS = "ReviewOptions";

    public const string PHILOSOPHY = "philosophy";
    public const string GENERATE = "generate";

    // network stuffs
    public const string APPLICATION_OCTET_STREAM = "application/octet-stream";
    
    // auth
    public const string AUTHORIZED_CLIENT_NAME = "authorizedClient";
    public const string LOG_IN_PATH = "LogInPath";
    public const string HEADER_NAME = "X-XSRF-TOKEN";
    public const string COOKIE_NAME = "__Host-X-XSRF-TOKEN";
    public const string XML_HTTP_REQUEST_HEADER = "XMLHttpRequest";
    public const string APPLICATION_JSON = "application/json";

    // environment variables
    public const string OPENAI_API_KEY = "OPENAI_API_KEY";
    
    // configuration
    public const string IDENTITY_OPTIONS = "IdentityOptions";
    public const string WORKSPACE_OPTIONS = "WorkspaceOptions";
    public const string REDIS_OPTIONS = "RedisOptions";
    public const string GPT_OPTIONS = "GptOptions";
    public const string AI_OPTIONS = "AIOptions";
    public const string CERTIFICATE_OPTIONS = "CertificateOptions";
    public const string HOST_OPTIONS = "HostOptions";
    public const string NO_CERTIFICATE_FOUND =
        "CertificateOptions were found however no X509CertificateCollection was found matching the values in the CertificateOptions";
}