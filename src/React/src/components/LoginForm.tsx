import { useState, type FormEvent, useEffect } from 'react';
import { login } from '../api';

export function LoginForm({ onLogin }: { onLogin?: (username: string, password: string) => void }) {
  const [username, setUsername] = useState('a@b.com');
  const [password, setPassword] = useState('5Babylon!');
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (onLogin) {
      login(username, password)
        .then(() => {
          onLogin(username, password);
        })
        .catch(() => {
          setError('Login failed');
        });
    }
  }, [username, password, onLogin]);

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    if (!username || !password) {
      setError('Please enter both username and password.');
      return;
    }
    setError(null);
  };

  return (
    <form onSubmit={handleSubmit} className="login-form">
      <div>
        <label htmlFor="username">Username</label>
        <input
          id="username"
          type="text"
          value={username}
          onChange={e => setUsername(e.target.value)}
          autoComplete="username"
        />
      </div>
      <div>
        <label htmlFor="password">Password</label>
        <input
          id="password"
          type="password"
          value={password}
          onChange={e => setPassword(e.target.value)}
          autoComplete="current-password"
        />
      </div>
      {error && <div className="error">{error}</div>}
      <button type="submit">Login</button>
    </form>
  );
}