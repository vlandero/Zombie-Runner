import { dbUrl } from "..";
import User from "../models/User";
import UserDTO from "../models/UserDTO";
import bcrypt from "bcryptjs";

export const getUser = async (username: string) => {
  const res = await fetch(`${dbUrl}/${username}.json`);
  if (res.status !== 200) {
    throw new Error("Error getting user");
  }
  const user = (await res.json()) as User | null;
  return user;
};

export const login = async ({
  username,
  password,
}: {
  username: string;
  password: string;
}) => {
  const user = await getUser(username);
  if (user === null) {
    throw new Error("Username or password is incorrect");
  }

  const passwordMatch = bcrypt.compareSync(password, user.password);

  if (!passwordMatch) {
    throw new Error("Username or password is incorrect");
  }

  return new UserDTO(user);
};
