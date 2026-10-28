namespace StayFlow.Communication.Responses;

public sealed class ResponseErrorJson
{
	public List<string> Errors { get; set; } = [];

	public bool AccessTokenExpired { get; set; }

	public ResponseErrorJson() { }
	public ResponseErrorJson(List<string> errorMessages) => Errors = errorMessages;
	public ResponseErrorJson(string errorMessage) => Errors = [errorMessage];
	public ResponseErrorJson(string errorMessage, bool accessTokenExpired)
	{
		Errors = [errorMessage];
		AccessTokenExpired = accessTokenExpired;
	}
}
