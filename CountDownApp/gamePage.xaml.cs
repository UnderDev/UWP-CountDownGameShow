﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CountDownApp
{

    public sealed partial class gamePage : Page
    {

        private char[] _vowlesArray = new char[5] { 'A', 'E', 'I', 'O', 'U'};
        private char[] _consonantsArray = new char[21] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R' , 'S', 'T', 'V', 'W', 'X', 'Y', 'Z'};
        private List<Char> _listAvailLetters = new List<char>();

        TextBox[] _textboxArray = new TextBox [9];

        private int _rndNum = 0 , _textBoxLoc = 0;


        public gamePage()
        {
            this.InitializeComponent();

            getTextBoxList();
        }


        //gets the amount of textboxes from gamePage and puts them into a list
        private void getTextBoxList()
        {
            for (int i = 0; i < _textboxArray.Length; i++)
            {
                _textboxArray[i] = ((TextBox)stkPanContiners.FindName("txtLetterBox" + i.ToString()));
            }
        }


        /*
        pass in the Vowels arrays length to the random generator
        then gets a random letter from the array and Outputs that
        letter in the aproperate box
        */
        private void btnVowels_Click(object sender, RoutedEventArgs e)
        {
            _rndNum = rndNumGen(_vowlesArray.Length);
            sendCharToTextBox((Char)_vowlesArray.GetValue(_rndNum));
        }

        /*
        Pass in the Consonants arrays length to the random generator.
        then gets a random letter from that array and outputs that 
        letter in the aproperate box
        */
        private void btnConsonants_Click(object sender, RoutedEventArgs e)
        {         
            _rndNum = rndNumGen(_consonantsArray.Length);
            sendCharToTextBox((Char)_consonantsArray.GetValue(_rndNum));
        }

        /*
        Get the location of the textbox and display the char passed in
        Also disable the 2 buttons that generate the letters
        */
        private void sendCharToTextBox(char _letter)
        {

            _listAvailLetters.Add(_letter);//Add the current letters to a list 
            _textboxArray[_textBoxLoc].Text = _letter.ToString();//send the textbox the letter

            //if all letters are added hide buttons
            if (_textBoxLoc == _textboxArray.Length-1)
            {
                _textBoxLoc = 0;
                btnVowels.Visibility = Visibility.Collapsed;
                btnConsonants.Visibility = Visibility.Collapsed;
                txtBoxUsrWord.Visibility = Visibility.Visible;

                btnCheckWord.Visibility = Visibility.Visible;
            }         
            else
                _textBoxLoc++;

            //START TIMER HERE
        }

        /*
        Generates a number between 0 and the number passed in
        */
        private int rndNumGen(int maxNo)
        {
             Random r = new Random();
            int   num = r.Next(maxNo);
            return num;
        }

        private void btnCheckWord_Click(object sender, RoutedEventArgs e)
        {
            if (chkLettersValidity())
            {
                chkWordValidity();
            }
            else
            {
                //Display a message to the user that its an INVALID WORD based on the givn letters
            }


            //RESET AFTER THE GAMES OVER
            resetGame();
        }

        /*Check that the letters entered by the user(converted to uppercase) are the letters that were displayed
        and that there are no duplicate letters used by the user when not available */
        private Boolean chkLettersValidity()
        {
            Boolean _validLetters = true;
            String _toUpper;
            int _leterIndex;
            //convert the users input to upper case
            _toUpper = txtBoxUsrWord.Text.ToUpper();

            //creates a new char array from the string to check for correct letters
            Char[] newArray = _toUpper.ToCharArray();

            //Check that the Word is using the same letters Displayed
            for (int i = 0; i < newArray.Length; i++)
            {
                //Finds the index of the current letter [i]  in the list _listAvailLetters, 
                _leterIndex = _listAvailLetters.IndexOf(newArray[i]);

                //If the letters not in the list
                if (_leterIndex == -1)
                {
                    _validLetters = false;
                }
                //Remove the letter from the list of available letters to stop duplicate letters 
                else
                    _listAvailLetters.RemoveAt(_leterIndex);
            }
            return _validLetters;
        }

        private void chkWordValidity()
        {
            //Searches the _wordsList for the word entered by the user(To lowercase) and gives its index
            int _index = App._wordsList.BinarySearch(txtBoxUsrWord.Text.ToLower());

            //if the word is found in _wordsList
            if (_index >= 0)
            {
                //Gets the words length and adds it to the users Score
                App._userScore = txtBoxUsrWord.Text.Length;

                stkPnlScoreBoard.Visibility = Visibility.Visible;
                txtBlockUsrScore.Text += ("    "+txtBoxUsrWord.Text+" \t\t"+ App._userScore +"\n");
            }
        }

        /*
        TextBox Checks all input from the keyboard to see if the user hit Enter key, if they did call the 
        btnCheckWord_Click button
        */
        private void txtBoxUsrWord_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                btnCheckWord_Click(sender,e);
            }
        }

        /*
        Resets the games: Buttons, and emptys the textboxes containing letters
        */
        private void resetGame()
        {
            btnVowels.Visibility = Visibility.Visible;
            btnConsonants.Visibility = Visibility.Visible;

            txtBoxUsrWord.Text = "";

            btnCheckWord.Visibility = Visibility.Collapsed;
            txtBoxUsrWord.Visibility = Visibility.Collapsed;

            //Reset all the textboxes to have nothing in them
            foreach (TextBox t in _textboxArray) {
                _textboxArray[_textBoxLoc++].Text = "";
                _listAvailLetters.Clear();//Clear the list
            }
            _textBoxLoc = 0;
        }

    }
}
