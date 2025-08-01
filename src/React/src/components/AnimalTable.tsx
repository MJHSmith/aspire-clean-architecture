import { useState } from "react";
import { deleteAnimal } from "../api";
import { AnimalDetailsModal } from "./AnimalDetailsModal";
import type { Animal } from "../types/Animal";

export function AnimalTable({ animals, onDelete }: { animals: Animal[]; onDelete: () => void }) {
  const [selectedAnimal, setSelectedAnimal] = useState<Animal | null>(null);

  function makeSound(sound: string) {
    if (window.speechSynthesis) {
      const utterance = new window.SpeechSynthesisUtterance(sound);
      window.speechSynthesis.speak(utterance);
    } else {
      alert(sound);
    }
  }

  return (
    <>
      <table>
        <thead>
          <tr>
            <th>Details</th>
            <th>Type</th>
            <th>Name</th>
            <th>Sound</th>
            <th>Delete</th>
          </tr>
        </thead>
        <tbody>
          {animals.map((a) => (
              <tr key={a.id}>
              <td data-label="Details">
                <button className="button-link" onClick={() => setSelectedAnimal(a)}>
                    Show
                </button>
              </td>
              <td data-label="Type">{a.type}</td>
              <td data-label="Name">{a.name}</td>
              <td data-label="Sound">
                
                <button onClick={async () => { makeSound(a.sound ?? ""); }} title="Play sound">
                  &#128266;
                </button>
                &nbsp;{a.sound}
              </td>
              <td data-label="Delete">
                <button
                  onClick={async () => {
                    await deleteAnimal(a.type, a.id);
                    onDelete();
                  }}
                >
                  &#x2715;
                </button>
              </td>

            </tr>
          ))}
        </tbody>
      </table>
      <AnimalDetailsModal animal={selectedAnimal} onClose={() => setSelectedAnimal(null)} />
    </>
  );
}