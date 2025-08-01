export interface Animal {
  id: string;
  type: string;
  name: string;
  sound?: string;
  [key: string]: unknown;
}
