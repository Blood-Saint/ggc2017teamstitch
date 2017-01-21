﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHubCrowdRow : MonoBehaviour, IHub
{
	[SerializeField] private UIHubCrowdIndividual _crowdIndividualPrefab;
	[SerializeField] private int _rowId;
	[SerializeField] private float _spacing;

	private List<UIHubCrowdIndividual> _individuals = new List<UIHubCrowdIndividual>();

	public void OnLevelChange( AppManager app )
	{
		for( int i=0; i<_individuals.Count; ++i )
		{
			Destroy( _individuals[i].gameObject );
		}

		_individuals.Clear();

		GameState gameState = app.GameStateManager.GameState;
		CrowdRow rowState = gameState.m_rows[_rowId];

		float stagger = 0.5f * (_rowId % 2);

		float leftTheta = 180.0f - gameState.m_thetaRange/2;
		float rightTheta = 180.0f + gameState.m_thetaRange/2;
		for( int i=0; i<rowState.m_individuals.Count; ++i )
		{
			GameObject go = GameObject.Instantiate(_crowdIndividualPrefab.gameObject);
			go.name = "Individual " + (i+1).ToString();
			go.transform.SetParent( transform );
			float x = (stagger + i) * _spacing;
			go.transform.localPosition = new Vector3( x, 0, 0 );
		
			float percent = x / 900; // not sure how to derive this; fuck it.
			gameState.m_rows[_rowId].m_individuals[i].m_waveTheta = percent * leftTheta + (1-percent) * rightTheta;

			UIHubCrowdIndividual uiHCI = go.GetComponent<UIHubCrowdIndividual>();
			uiHCI.Configure( _rowId, i );
			uiHCI.OnLevelChange( app );
			_individuals.Add( uiHCI );
		}

	}

	public void UI( AppManager app )
	{
		for( int i=0; i<_individuals.Count; ++i )
		{
			_individuals[i].UI( app );
		}
	}
}
