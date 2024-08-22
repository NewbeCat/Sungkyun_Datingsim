using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class TypeEffect : MonoBehaviour
{
    // Basic Typewriter Functionality
    private TextMeshProUGUI _textBox;
    private int _currentVisibleCharacterIndex;
    private int maxlength = 0;

    private Coroutine _typewriterCoroutine;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interpunctuationDelay;

    [Header("Typewriter Settings")]
    [SerializeField] private float charactersPerSecond = 20;
    [SerializeField] private float interpunctuationDelay = 0.10f;


    // Skipping Functionality
    public bool CurrentlySkipping { get; private set; }
    private WaitForSeconds _skipDelay;

    [Header("Skip options")]
    [SerializeField] private bool quickSkip;
    [SerializeField][Min(1)] private int skipSpeedup = 5;


    // Event Functionality
    private WaitForSeconds _textboxFullEventDelay;
    [SerializeField][Range(0.1f, 0.5f)] private float sendDoneDelay = 0.25f;

    public event Action CompleteTextRevealed;
    public event Action<char> CharacterRevealed;


    private void Awake()
    {
        _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);

        _skipDelay = new WaitForSeconds(1 / (charactersPerSecond * skipSpeedup));
        _textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);
    }

    public void TypingNewText(TextMeshProUGUI textbox, string newstring)
    {

        _textBox = textbox;
        _textBox.text = newstring;

        CurrentlySkipping = false;

        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);

        _textBox.maxVisibleCharacters = 0;
        _currentVisibleCharacterIndex = 0;

        _typewriterCoroutine = StartCoroutine(Typewriter(newstring));
    }

    private IEnumerator Typewriter(string newstring)
    {
        maxlength = newstring.Length;

        while (_currentVisibleCharacterIndex <= maxlength)
        {
            var lastCharacterIndex = maxlength;

            if (_currentVisibleCharacterIndex >= lastCharacterIndex)
            {
                _textBox.maxVisibleCharacters++;
                yield return _textboxFullEventDelay;
                CompleteTextRevealed?.Invoke();
                yield break;
            }

            char character = newstring[_currentVisibleCharacterIndex];

            _textBox.maxVisibleCharacters++;

            if (!CurrentlySkipping &&
                (character == '?' || character == '.' || character == ',' || character == ':' ||
                 character == ';' || character == '!' || character == '-'))
            {
                yield return _interpunctuationDelay;
            }
            else
            {
                yield return CurrentlySkipping ? _skipDelay : _simpleDelay;
            }

            CharacterRevealed?.Invoke(character);
            _currentVisibleCharacterIndex++;
        }
    }

    public void Skip(bool quickSkipNeeded = false)
    {
        if (CurrentlySkipping)
            return;

        CurrentlySkipping = true;

        StopCoroutine(_typewriterCoroutine);
        _textBox.maxVisibleCharacters = maxlength;
        CompleteTextRevealed?.Invoke();
    }
}