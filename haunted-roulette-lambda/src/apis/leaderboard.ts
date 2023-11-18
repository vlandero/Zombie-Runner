import { dbUrl } from "..";
import User from "../models/User";
import UserDTO from "../models/UserDTO";

export const getAllEntries = async () => {
  const res = await fetch(`${dbUrl}/.json`);
  if (res.status !== 200) {
    throw new Error("Error getting leaderboard");
  }
  const entries = (await res.json()) as { [key: string]: User } | null;
  if (entries === null) {
    return [];
  }
  return Object.values(entries).map((entry) => new UserDTO(entry));
};

export const getLeaderboard = async () => {
  const entries = await getAllEntries();
  const leaderboard = entries.sort((a, b) => b.highscore - a.highscore);
  return leaderboard;
};
