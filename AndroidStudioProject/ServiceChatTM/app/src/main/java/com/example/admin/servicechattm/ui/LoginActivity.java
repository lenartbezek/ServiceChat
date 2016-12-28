package com.example.admin.servicechattm.ui;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import com.example.admin.servicechattm.R;

public class LoginActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        final EditText etUsername = (EditText) findViewById(R.id.etUsername);
        final EditText etPassword = (EditText) findViewById(R.id.etPassword);
        final Button bLogin = (Button) findViewById((R.id.bLogin));

        bLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                final String username = etUsername.getText().toString();
                final String password = etPassword.getText().toString();

                //za testirat brez servisa
                Intent intent = new Intent(LoginActivity.this, MessageActivity.class);
                LoginActivity.this.startActivity(intent);

                /*
                response listener; se proba povezat na servis ki bi mu mogu vrnit json glede na vnesen username in pw (pac true/false). Ce rata ok gre na recycler view, cene vrze error uporabniku.
                potem je se dodano da naj bi mu json vrnu se ime in priimek tko da ga lahko uporabim v recycler view
                */

/*
                Response.Listener<String> responseListener = new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {
                        try {
                            JSONObject jsonResponse = new JSONObject(response);
                            boolean success = jsonResponse.getBoolean("success");
                            if(success){
                                String name = jsonResponse.getString("name");
                                String surname = jsonResponse.getString("surname");
                                Intent intent = new Intent(LoginActivity.this, MsgActivity.class);
                                intent.putExtra("name", name);
                                intent.putExtra("surname", surname);
                                LoginActivity.this.startActivity(intent);
                            }else{
                                AlertDialog.Builder builder = new AlertDialog.Builder(LoginActivity.this);
                                builder.setMessage("Login failed, check your username and password.")
                                        .setNegativeButton("Ok", null)
                                        .create()
                                        .show();
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }

                    }
                };

                LoginRequest loginRequest = new LoginRequest(username, password, responseListener);
                RequestQueue queue = Volley.newRequestQueue(LoginActivity.this);
                queue.add(loginRequest);

*/


            }
        });

    }
}
