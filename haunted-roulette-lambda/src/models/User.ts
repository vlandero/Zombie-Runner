export default class User {
  username: string;
  password: string;
  highscore: number;
  constructor(username: string, password: string) {
    this.username = username;
    this.password = password;
    this.highscore = 0;
  }
}
