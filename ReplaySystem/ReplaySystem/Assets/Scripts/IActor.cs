using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor {
	void ProcessCommand(BaseCommand command);
	void UndoCommand(BaseCommand command);
	void ProcessUpdate(float timeMultiplier);
	void ResetActor();
}