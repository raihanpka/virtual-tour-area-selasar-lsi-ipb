using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.VRTemplate
{
    public class QuizManager : MonoBehaviour
    {
        /* ---------- DATA ---------- */
        [Serializable]
        class Question
        {
            public GameObject panel;          // panel/card soal
        }

        [Header("PANEL AWAL")]
        [SerializeField] GameObject    startPanel;        // panel intro
        [SerializeField] Button        startButton;       // tombol “Mulai”

        [Header("Daftar Soal")]
        [SerializeField] List<Question> questions = new();

        [Header("Panel Feedback")]
        [SerializeField] GameObject correctPanel;
        [SerializeField] GameObject wrongPanel;
        [SerializeField] Button     correctNextBtn;
        [SerializeField] Button     wrongNextBtn;

        [Header("Ringkasan")]
        [SerializeField] GameObject     summaryPanel;
        [SerializeField] TextMeshProUGUI summaryText;
        [SerializeField] Button         retryButton;

        /* ---------- STATE ---------- */
        int currentIndex;
        int correctCount;
        int wrongCount;

        /* ---------- INIT ---------- */
        void Start()
        {
            // Bersih-bersih listener lama
            startButton   .onClick.RemoveAllListeners();
            correctNextBtn.onClick.RemoveAllListeners();
            wrongNextBtn  .onClick.RemoveAllListeners();
            retryButton   .onClick.RemoveAllListeners();

            // Pasang listener
            startButton   .onClick.AddListener(BeginQuiz);
            correctNextBtn.onClick.AddListener(GoNext);
            wrongNextBtn  .onClick.AddListener(GoNext);
            retryButton   .onClick.AddListener(Restart);

            // Tampilkan hanya Panel Awal
            HideAllContent();
            startPanel.SetActive(true);
        }

        /* ---------- PUBLIC (dipanggil tombol soal) ---------- */
        public void Answer(bool isCorrect)
        {
            HideAllContent();

            if (isCorrect)
            {
                ++correctCount;
                correctPanel.SetActive(true);
            }
            else
            {
                ++wrongCount;
                wrongPanel.SetActive(true);
            }
        }

        /* ---------- PRIVATE FLOW ---------- */
        void BeginQuiz()
        {
            // Reset skor & index saat pertama kali mulai
            correctCount = wrongCount = 0;
            currentIndex  = 0;
            ShowQuestion(currentIndex);
        }

        void GoNext()
        {
            HideAllContent();
            ++currentIndex;

            if (currentIndex >= questions.Count)
            {
                ShowSummary();
            }
            else
            {
                ShowQuestion(currentIndex);
            }
        }

        void ShowQuestion(int index)
        {
            HideAllContent();

            for (int i = 0; i < questions.Count; ++i)
                questions[i].panel.SetActive(i == index);
        }

        void ShowSummary()
        {
            HideAllContent();
            summaryPanel.SetActive(true);
            summaryText.text =
                $"Benar : {correctCount}\n" +
                $"Salah : {wrongCount}";
            retryButton.gameObject.SetActive(true);
        }

        void Restart()
        {
            // Kembali ke Panel Awal
            HideAllContent();
            startPanel.SetActive(true);
        }

        /* ---------- UTIL ---------- */
        void HideAllContent()
        {
            // Panel intro
            startPanel.SetActive(false);

            // Panel soal
            foreach (var q in questions)
                q.panel.SetActive(false);

            // Panel lainnya
            correctPanel.SetActive(false);
            wrongPanel.SetActive(false);
            summaryPanel.SetActive(false);
            retryButton.gameObject.SetActive(false);
        }
    }
}
