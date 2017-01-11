package com.example.admin.servicechattm;

import android.content.Context;
import android.util.Base64;

import java.io.*;

public class Auth {

    private static final String TOKEN_PATH = "authtoken.txt";
    private static String token;

    /**
     * Prebere in vrne token iz lokalnega pomnilnika.
     * @return Http BasicAuth token.
     * @throws TokenNotFoundException če ne najde tokena.
     */
    public static String getToken() throws TokenNotFoundException {
        if (token == null){
            try {
                FileInputStream fis = ServiceChatApp.getContext().openFileInput(TOKEN_PATH);

                InputStreamReader inputStreamReader = new InputStreamReader(fis);
                BufferedReader bufferedReader = new BufferedReader(inputStreamReader);

                String receiveString = "";
                StringBuilder stringBuilder = new StringBuilder();

                while ( (receiveString = bufferedReader.readLine()) != null ) {
                    stringBuilder.append(receiveString);
                }

                fis.close();
                inputStreamReader.close();

                return stringBuilder.toString();
            } catch (IOException e){
                throw new TokenNotFoundException();
            }
        } else {
            return token;
        }
    }

    /**
     * Shrani token v lokalni pomnilnik. Klicano po uspešnem loginu.
     * @param newToken Vsebina novega tokena.
     * @return True če je bilo uspešno.
     */
    public static boolean setToken(String newToken){
        token = newToken;
        try{
            FileOutputStream fos = ServiceChatApp.getContext().openFileOutput(TOKEN_PATH, Context.MODE_PRIVATE);
            fos.write(token.getBytes());
            fos.flush();
            fos.close();
            return true;
        } catch (IOException e){
            e.printStackTrace();
            return false;
        }
    }


    /**
     * Zakodira token iz uporabniškega imena in gesla ter ga shrani v lokalni pomnilnik. Klicano po uspešnem loginu.
     * @param username
     * @param password
     * @return True če je bilo uspešno.
     */
    public static boolean setToken(String username, String password){
        byte[] bytes = (username+":"+password).getBytes();
        return setToken("Basic "+Base64.encodeToString(bytes, Base64.DEFAULT));
    }

    /**
     * Izbriše token iz lokalnega pomnilnika. Klicano po log outu.
     * @return True če je bilo uspešno.
     */
    public static boolean deleteToken(){
        token = null;
        File file = new File(TOKEN_PATH);
        return file.delete();
    }

    public static class TokenNotFoundException extends Exception{
        @Override
        public String getMessage(){
            return "Auth token not found. User must be logged in first.";
        }
    }

}
