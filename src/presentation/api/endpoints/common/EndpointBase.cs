using Microsoft.AspNetCore.Mvc;

namespace api.endpoints.common;

[ApiController, Route("api")]
public abstract class EndpointBase : ControllerBase;