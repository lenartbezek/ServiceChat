package com.example.admin.servicechattm.ui;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;

import com.example.admin.servicechattm.R;
import com.example.admin.servicechattm.adapter.ServiceChatAdapter;
import com.example.admin.servicechattm.model.DummyData;
import com.example.admin.servicechattm.model.ListItem;

import java.util.ArrayList;
import java.util.List;

public class MessageActivity extends AppCompatActivity implements ServiceChatAdapter.ItemClickCallback{

    //Intent intent = getIntent();
    //String name = intent.getStringExtra("name");
    //String surname = intent.getStringExtra("surname");

    private static final String BUNDLE_EXTRAS = "BUNDLE_EXTRAS";
    private static final String EXTRA_QUOTE = "EXTRA_QUOTE";
    private static final String EXTRA_ATTR = "EXTRA_ATTR";


    private RecyclerView recView;
    private ServiceChatAdapter adapter;
    private ArrayList listData;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_message);

        listData = (ArrayList)DummyData.getListData();

        recView = (RecyclerView)findViewById(R.id.recList);
        recView.setLayoutManager(new LinearLayoutManager(this));

        adapter = new ServiceChatAdapter(DummyData.getListData(), this);
        recView.setAdapter(adapter);
        adapter.setItemClickCallback(this);
    }

    @Override
    public void onItemClick(int p) {
        ListItem item = (ListItem) listData.get(p);

        Intent i = new Intent(this, DetailActivity.class);

        Bundle extras = new Bundle();
        extras.putString(EXTRA_QUOTE, item.getTitle());
        extras.putString(EXTRA_ATTR, item.getName());

        i.putExtra(BUNDLE_EXTRAS, extras);
        startActivity(i);
    }

    @Override
    public void onSecondaryIconClick(int p) {
        ListItem item = (ListItem) listData.get(p);
        if(item.isFavorite()){
            item.setFavorite(false);
        }else{
            item.setFavorite(true);
        }
        adapter.setListData(listData);
        adapter.notifyDataSetChanged();
    }
}
