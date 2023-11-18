import { APIGatewayProxyEventV2, Context, Handler } from "aws-lambda";
import { register } from "./apis/register";
import { getSSMParameter } from "./utils/ssm";
import { login } from "./apis/login";
import UserDTO from "./models/UserDTO";
import { getLeaderboard } from "./apis/leaderboard";

class ApiResponse {
  statusCode: number;
  error: boolean;
  errorMessage?: string;
  body: {
    error: boolean;
    payload?: string;
  };
  headers?: { [key: string]: string } = {};
  constructor({
    statusCode,
    body,
    headers,
    error,
    errorMessage,
  }: {
    statusCode: number;
    body: {
      error: boolean;
      payload?: string;
    };
    headers?: { [key: string]: string };
    error?: boolean;
    errorMessage?: string;
  }) {
    this.statusCode = statusCode;
    this.body = body;
    this.headers = headers;
    this.error = error || false;
    this.errorMessage = errorMessage;
  }
}

export let authUrl: string | null = null;
export let apiKey: string | null = null;
export let authEmail: string | null = null;
export let authPassword: string | null = null;
export let dbUrl: string | null = null;

export const handler: Handler = async (
  event: APIGatewayProxyEventV2,
  context: Context
) => {
  authUrl =
    authUrl ||
    (await getSSMParameter("/haunder-fortune/api/firebase/auth-url"));
  apiKey =
    apiKey || (await getSSMParameter("/haunder-fortune/api/firebase/api-key"));
  authEmail =
    authEmail ||
    (await getSSMParameter("/haunder-fortune/api/firebase/auth-email"));
  authPassword =
    authPassword ||
    (await getSSMParameter("/haunder-fortune/api/firebase/auth-password"));
  dbUrl =
    dbUrl || (await getSSMParameter("/haunder-fortune/api/firebase/db-url"));
  const route = event.rawPath;
  console.log(event);
  try {
    if (route === "/login" && event.requestContext.http.method === "POST") {
      const { username, password } = JSON.parse(event.body || "{}");
      const user = await login({ username, password });
      return new ApiResponse({
        statusCode: 200,
        body: { error: false, payload: JSON.stringify(user) },
      });
    } else if (
      route === "/create-user" &&
      event.requestContext.http.method === "POST"
    ) {
      const { username, password } = JSON.parse(event.body || "{}");
      const dto = await register({ username, password });
      return new ApiResponse({
        statusCode: 200,
        body: {
          error: false,
          payload: JSON.stringify(dto),
        },
      });
    } else if (
      route === "/get-leaderboard" &&
      event.requestContext.http.method === "GET"
    ) {
      const leaderboard = await getLeaderboard();
      return new ApiResponse({
        statusCode: 200,
        body: { error: false, payload: JSON.stringify(leaderboard) },
      });
    } else {
      return new ApiResponse({
        statusCode: 404,
        body: { error: true, payload: "Not found" },
      });
    }
  } catch (err: any) {
    console.log(JSON.stringify(err));
    return new ApiResponse({
      statusCode: 200, // not the purpose of the project to handle errors by status code 400 and message in the game
      body: { error: true, payload: err.message },
    });
  }
};
