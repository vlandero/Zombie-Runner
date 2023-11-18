import {
  SSMClient,
  GetParameterCommand,
  GetParameterCommandOutput,
} from "@aws-sdk/client-ssm";

export const getSSMParameter = async (key: string): Promise<string | null> => {
  const ssm = new SSMClient();
  const params = {
    Name: key,
    WithDecryption: true,
  };

  try {
    const response: GetParameterCommandOutput = await ssm.send(
      new GetParameterCommand(params)
    );
    return response.Parameter?.Value || null;
  } catch (error) {
    console.error("Error retrieving SSM parameter:", error);
    return null;
  }
};
