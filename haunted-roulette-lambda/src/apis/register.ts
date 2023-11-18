import { apiKey, authEmail, authPassword, authUrl, dbUrl } from "..";
import User from "../models/User";
import UserDTO from "../models/UserDTO";
import { getUser } from "./login";
import bcrypt from "bcryptjs";

export const register = async ({
  username,
  password,
}: {
  username: string;
  password: string;
}) => {
  const existingUser = await getUser(username);
  if (existingUser !== null) {
    throw new Error("User already exists");
  }

  const hashedPassword = bcrypt.hashSync(password, 10);

  const res = await fetch(`${authUrl}?key=${apiKey}`, {
    body: JSON.stringify({
      email: authEmail,
      password: authPassword,
      returnSecureToken: true,
    }),
    headers: {
      "Content-Type": "application/json",
    },
    method: "POST",
  });

  if (res.status !== 200) {
    throw new Error("Error creating user");
  }

  const { idToken }: { idToken: string } = (await res.json()) as any;

  const user = await fetch(`${dbUrl}/${username}.json?auth=${idToken}`, {
    body: JSON.stringify({
      username,
      password: hashedPassword,
      highscore: 0,
    }),
    headers: {
      "Content-Type": "application/json",
    },
    method: "PUT",
  });

  const userJson = (await user.json()) as User;
  return new UserDTO(userJson);
};
