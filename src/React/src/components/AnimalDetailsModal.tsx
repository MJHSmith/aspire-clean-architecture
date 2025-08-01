import React, { useEffect, useRef, useState } from "react";
import styles from './AnimalDetailsModal.module.css';
import { fetchAnimal } from "../api";
import type { Animal } from "../types/Animal";

export function AnimalDetailsModal({
  animal,
  onClose,
}: {
  animal: Animal | null;
  onClose: () => void;
}) {
  const dialogRef = useRef<HTMLDialogElement>(null);
  const [details, setDetails] = useState<Animal | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (animal && dialogRef.current) {
      dialogRef.current.showModal();
      setLoading(true);
      setError(null);
      fetchAnimal(animal.type, animal.id)
        .then(data => setDetails(data))
        .catch(() => setError("Failed to load details"))
        .finally(() => setLoading(false));
    } else if (dialogRef.current) {
      dialogRef.current.close();
      setDetails(null);
      setError(null);
    }
  }, [animal]);

  if (!animal) return null;

  return (
    <dialog ref={dialogRef} onClose={onClose} className={styles.dialog}>
      <form method="dialog" className="animal-modal-form">
        <button
          type="button"
          onClick={onClose}
          className="dialog-close"
          aria-label="Close"
        >
          &#x2715;
        </button>
        <h2>Animal Details</h2>
        {loading && <div>Loading...</div>}
        {error && <div style={{ color: "red" }}>{error}</div>}
        {details && (
          <ul>
            {Object.entries(details).map(([key, value]) => (
              <li key={key} id={styles[key] || undefined}>
                <strong className={styles.detailsKey}>{key}:</strong>
                <span className={styles.detailsValue}>{String(value)}</span>
              </li>
            ))}
          </ul>
        )}
      </form>
    </dialog>
  );
}