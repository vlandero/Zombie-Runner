import { dbUrl } from "..";
import { getAllEntries } from "./leaderboard";
import { getToken } from "./register";

export const updateHighScore = async (username: string, highscore: number) => {
  const entries = await getAllEntries();
  const user = entries.find((entry) => entry.username === username);
  if (user === undefined) {
    throw new Error("User not found");
  }
  user.highscore = highscore > user.highscore ? highscore : user.highscore;
  const idToken = await getToken();
  const res = await fetch(`${dbUrl}/${username}.json?auth=${idToken}`, {
    body: JSON.stringify(user),
    headers: {
      "Content-Type": "application/json",
    },
    method: "PUT",
  });
  console.log(res);
  if (res.status !== 200) {
    throw new Error("Error updating highscore");
  }
  return user;
};
