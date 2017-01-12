package com.example.admin.servicechattm;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import com.example.admin.servicechattm.model.Message;
import com.example.admin.servicechattm.model.User;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import org.joda.time.format.DateTimeFormat;
import org.joda.time.format.DateTimeFormatter;

import java.util.TimeZone;

public class MessageAdapter extends RecyclerView.Adapter<MessageAdapter.MessageViewHolder> {

    private final static DateTimeFormatter todayTimeFormat = DateTimeFormat.forPattern("HH':'mm");
    private final static DateTimeFormatter weekTimeFormat = DateTimeFormat.forPattern("EEEEE");
    private final static DateTimeFormatter otherTimeFormat = DateTimeFormat.forPattern("MMMMM dd");

    private final static long MILLIS_PER_DAY = 24 * 60 * 60 * 1000L;
    private final static long MILLIS_PER_WEEK = 7 * MILLIS_PER_DAY;

    private Context context;

    public static class MessageViewHolder extends RecyclerView.ViewHolder {

        protected TextView messageTextView;
        protected TextView authorTextView;
        protected TextView timeTextView;
        protected Context context;

        public MessageViewHolder(View v) {
            super(v);
            context = itemView.getContext();

            messageTextView = (TextView) v.findViewById(R.id.messageTextView);
            authorTextView = (TextView) v.findViewById(R.id.authorTextView);
            timeTextView = (TextView) v.findViewById(R.id.timeTextView);
        }
    }

    // Create new views (invoked by the layout manager)
    @Override
    public MessageViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        // create a new view
        View v = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.message_layout, parent, false);
        v.setPadding(8, 8, 8, 8);

        return new MessageViewHolder(v);
    }

    // Replace the contents of a view (invoked by the layout manager)
    @Override
    public void onBindViewHolder(MessageViewHolder holder, int i) {

        Message message = Message.getAll().get(i);
        User user = User.get(message.username);

        holder.messageTextView.setText(message.text);
        holder.authorTextView.setText(user != null ? user.displayName : "...");

        long localTime = message.time.withZone(DateTimeZone.getDefault()).getMillis();
        long age = DateTime.now().getMillis() - localTime;
        if (age < MILLIS_PER_DAY)
            holder.timeTextView.setText(todayTimeFormat.print(localTime));
        else if (age < MILLIS_PER_WEEK)
            holder.timeTextView.setText(weekTimeFormat.print(localTime));
        else
            holder.timeTextView.setText(otherTimeFormat.print(localTime));
    }

    // Return the size of your dataset (invoked by the layout manager)
    @Override
    public int getItemCount() {
        return Message.count();
    }
}
