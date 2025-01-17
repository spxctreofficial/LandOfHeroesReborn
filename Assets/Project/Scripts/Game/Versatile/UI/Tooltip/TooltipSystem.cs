﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
	public static TooltipSystem instance;

	public enum TooltipType { Tooltip, ErrorTooltip, CardTooltip }
	public Tooltip tooltip;
	public FixedTooltip fixedTooltip;
	public CardTooltip cardTooltip;

	public List<int> currentlyActiveIDs = new List<int>();

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
		{
			Destroy(gameObject);
		}
	}

	public void Show(string body, string header = "")
	{
		tooltip.UpdateTransform();
		tooltip.UpdatePivot();

		tooltip.SetText(body, header);
		tooltip.canvasGroup.alpha = 0f;
		LeanTween.alphaCanvas(tooltip.canvasGroup, 1f, 0.25f).setEaseInOutQuart();
	}
	public void ShowCard(Card card)
    {
		cardTooltip.Setup(card);

		cardTooltip.transform.localScale = new Vector2(0.8f, 0.8f);
		cardTooltip.canvasGroup.alpha = 0f;
		LeanTween.scale(cardTooltip.rectTransform, Vector2.one, 0.3f).setEaseInOutQuart();
		LeanTween.alphaCanvas(cardTooltip.canvasGroup, 1f, 0.3f).setEaseInOutQuart();
	}
	public void ShowError(string body, string header = "")
	{
		fixedTooltip.UpdateTransform();
		fixedTooltip.UpdatePivot();

		fixedTooltip.SetText(body, header);
		fixedTooltip.canvasGroup.alpha = 0f;
		LeanTween.alphaCanvas(fixedTooltip.canvasGroup, 1f, 0.25f).setEaseInOutQuart();
	}
	/// <summary>
	/// Hide all active tooltips.
	/// </summary>
	public void Hide()
	{
		tooltip.GetComponent<CanvasGroup>().alpha = 0f;

		fixedTooltip.GetComponent<CanvasGroup>().alpha = 0f;

		cardTooltip.GetComponent<CanvasGroup>().alpha = 0f;
	}
	/// <summary>
	/// Hide a specific type of tooltip.
	/// </summary>
	/// <param name="tooltipType"></param>
	public void Hide(TooltipType tooltipType)
	{
		float fadeOutTime = 0.1f;

		switch (tooltipType)
		{
			case TooltipType.Tooltip:
				LeanTween.alphaCanvas(tooltip.canvasGroup, 0f, fadeOutTime).setEaseInOutQuart();
				break;
			case TooltipType.ErrorTooltip:
				LeanTween.alphaCanvas(fixedTooltip.canvasGroup, 0f, fadeOutTime).setEaseInOutQuart();
				break;
			case TooltipType.CardTooltip:
				if (currentlyActiveIDs.Count > 0 || cardTooltip.canvasGroup.alpha == 0f) break;
				currentlyActiveIDs.Add(LeanTween.scale(cardTooltip.rectTransform, new Vector2(0.8f, 0.8f), 0.3f).setEaseInOutQuart().uniqueId);
				currentlyActiveIDs.Add(LeanTween.alphaCanvas(cardTooltip.canvasGroup, 0f, 0.3f).setEaseInOutQuart().setOnComplete(() => currentlyActiveIDs.Clear()).uniqueId);
				break;
		}
	}
}
