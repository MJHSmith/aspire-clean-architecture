import { useState } from "react";
import { createAnimal } from "../api";
import type { Animal } from "../types/Animal";

export function AnimalForm({ onCreated }: { onCreated: () => void }) {
  const [type, setType] = useState<Animal["type"]>("Cat");
  const [fields, setFields] = useState<Record<string, string>>({});

  function handleChange(e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
    setFields({ ...fields, [e.target.name]: e.target.value });
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    await createAnimal(type, fields);
    setFields({});
    onCreated();
  }

  return (
    <form className="animal-form" onSubmit={handleSubmit}>
      <select name="type" value={type} onChange={e => setType(e.target.value as Animal["type"])}
      >
        <option value="Cat">Cat</option>
        <option value="Dog">Dog</option>
        <option value="Bird">Bird</option>
      </select>
      <input name="name" type="text" placeholder="Name" value={fields.name || ""} onChange={handleChange} required />
      {type === "Cat" && (
        <input name="hairColourDescription" type="text" placeholder="Hair Colour Description" value={fields.hairColourDescription || ""} onChange={handleChange} required />
      )}
      {type === "Dog" && (
        <>
          <input name="hairColour1" type="text" placeholder="Hair Colour 1" value={fields.hairColour1 || ""} onChange={handleChange} required />
          <input name="hairColour2" type="text" placeholder="Hair Colour 2" value={fields.hairColour2 || ""} onChange={handleChange} />
          <input name="hairPattern" type="text" placeholder="Hair Pattern" value={fields.hairPattern || ""} onChange={handleChange} />
        </>
      )}
      {type === "Bird" && (
        <input name="wingspanInCentimeters" placeholder="Wingspan (cm)" type="number" value={fields.wingspanInCentimeters || ""} onChange={handleChange} required />
      )}
      <button type="submit">Create</button>
    </form>
  );
}