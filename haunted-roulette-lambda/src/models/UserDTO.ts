import User from "./User";

export default class UserDTO {
  username: string;
  highscore: number;
  constructor(user: User) {
    this.username = user.username;
    this.highscore = user.highscore;
  }
}
